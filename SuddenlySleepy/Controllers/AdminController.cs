using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuddenlySleepy.Models;
using Microsoft.AspNetCore.Authorization;

namespace SuddenlySleepy.Controllers
{
    [Authorize(Roles = "_Admin")] // only admin can access admin stuff
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<SSUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleMgr, UserManager<SSUser> userMgr)
        {
            _roleManager = roleMgr;
            _userManager = userMgr;
        }

        public IActionResult AdminUserManagement()
        {
            return View(_userManager.Users);
        }

        public IActionResult AdminRoleManagement()
        {
            ViewBag.userManager = _userManager;
            return View(_roleManager.Roles);
        }

        [HttpPost]
        public async Task<IActionResult> AdminDeleteUser(string id)
        {
            SSUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminUserManagement", _userManager.Users);
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("AdminUserManagement", _userManager.Users);
        }

        // I don't have time to master editing users for this project

        public async Task<IActionResult> AdminEditUser(string id)
        {
            SSUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("AdminUserManagement");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdminEditUser(SSUser editedUser)
        {
            SSUser user = await _userManager.FindByIdAsync(editedUser.Id);
            if (user != null)
            {
                {
                    if (user.UserName != editedUser.UserName)
                        user.UserName = editedUser.UserName;
                    if (user.FirstName != editedUser.FirstName)
                        user.FirstName = editedUser.FirstName;
                    if (user.LastName != editedUser.LastName)
                        user.LastName = editedUser.LastName;
                    if (user.Email != editedUser.Email)
                        user.Email = editedUser.Email;

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("AdminUserManagement");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }


        public IActionResult AdminCreateRole() => View();

        [HttpPost]
        public async Task<IActionResult> AdminCreateRole([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminRoleManagement");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> AdminDeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminRoleManagement");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "No role found");
            }
            return View("AdminRoleManagement", _roleManager.Roles);
        }

        public async Task<IActionResult> AdminEditRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<SSUser> members = new List<SSUser>();
            List<SSUser> nonMembers = new List<SSUser>();
            foreach (SSUser user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> AdminEditRole(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    SSUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    SSUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(AdminRoleManagement));
            }
            else
            {
                return await AdminEditRole(model.RoleId);
            }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}