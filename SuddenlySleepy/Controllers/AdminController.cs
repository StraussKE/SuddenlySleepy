using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuddenlySleepy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace SuddenlySleepy.Controllers
{
    [Authorize(Roles = "_Admin")] // only admin can access admin stuff
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<SSUser> _userManager;
        private readonly AppIdentityDbContext _context;


        public AdminController(RoleManager<IdentityRole> roleMgr, UserManager<SSUser> userMgr, AppIdentityDbContext ctx)
        {
            _roleManager = roleMgr;
            _userManager = userMgr;
            _context = ctx;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// User Management Section
        /// </summary>

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

        /// <summary>
        /// Role Management Section
        /// </summary>

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

        /// <summary>
        /// Donation Management Section
        /// </summary>

        // GET: Admin/AdminDonationRecord
        public async Task<IActionResult> AdminDonationRecord(string orderBy = "default")
        {
            ViewBag.orderBy = orderBy;
            if (orderBy == "amount")
            {
                return View(await _context.Donations.Include(d => d.Donor).OrderBy(d => d.DonationAmount).ToListAsync());
            }
            else if (orderBy == "revamount")
            {
                return View(await _context.Donations.Include(d => d.Donor).OrderByDescending(d => d.DonationAmount).ToListAsync());
            }
            else if(orderBy == "donor")
            {
                return View(await _context.Donations.Include(d => d.Donor).OrderBy(d => d.Donor.Id).ToListAsync());
            }
            else if (orderBy == "revdonor")
            {
                return View(await _context.Donations.Include(d => d.Donor).OrderByDescending(d => d.Donor.Id).ToListAsync());
            }
            else if (orderBy == "date")
            {
                return View(await _context.Donations.Include(d => d.Donor).OrderBy(d => d.DonationDate).ToListAsync());
            }
            else if (orderBy == "revdate")
            {
                return View(await _context.Donations.Include(d => d.Donor).OrderByDescending(d => d.DonationDate).ToListAsync());
            }
            else if (orderBy == "revdefault")
            {
                return View(await _context.Donations.Include(d => d.Donor).OrderByDescending(d => d.DonationId).ToListAsync());
            }
            else
            {
                return View(await _context.Donations.Include(d => d.Donor).OrderBy(d => d.DonationId).ToListAsync());
            }
        }

        // GET: Admin/AdminDonationDetails/5
        public async Task<IActionResult> AdminDonationDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.Include(d => d.Donor)
                .FirstOrDefaultAsync(v => v.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        // GET: Admin/AdminDonationDelete/5
        public async Task<IActionResult> AdminDonationDelete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Admin/AdminDonationDelete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDonationDeleteConfirmed(Guid id)
        {
            var donation = await _context.Donations.FindAsync(id);
            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminDonationRecord));
        }

        private bool DonationExists(Guid id)
        {
            return _context.Donations.Any(e => e.DonationId == id);
        }

        /// <summary>
        /// Event Management Section
        /// </summary>

        // GET: Admin/AdminEventManagement
        public async Task<IActionResult> AdminEventManagement()
        {
            return View(await _context.SSEvents.ToListAsync());
        }

        // GET: Admin/AdminNewEvent
        public IActionResult AdminNewEvent()
        {
            return View();
        }

        // POST: Admin/AdminNewEvent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminNewEvent([Bind("SSEventId,MeetingDate,Location,SSEventName,Description")] SSEvent sSEvent, string save = "no")
        {
            ViewBag.thisEvent = sSEvent;
            if (ModelState.IsValid && save == "save")
            {
                sSEvent.SSEventId = Guid.NewGuid();
                _context.Add(sSEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sSEvent);
        }

        // GET: SSEvents/Details/5
        public async Task<IActionResult> EventDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sSEvent = await _context.SSEvents
                .FirstOrDefaultAsync(m => m.SSEventId == id);
            if (sSEvent == null)
            {
                return NotFound();
            }

            return View(sSEvent);
        }




        // GET: SSEvents/Edit/5
        public async Task<IActionResult> AdminEditEvent(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sSEvent = await _context.SSEvents.FindAsync(id);
            if (sSEvent == null)
            {
                return NotFound();
            }
            return View(sSEvent);
        }

        // POST: SSEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEditEvent(Guid id, [Bind("SSEventId,MeetingDate,Location,SSEventName,Description")] SSEvent sSEvent)
        {
            if (id != sSEvent.SSEventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sSEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SSEventExists(sSEvent.SSEventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminEventManagement));
            }
            return View(sSEvent);
        }

        // GET: SSEvents/Delete/5
        public async Task<IActionResult> AdminDeleteEvent(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sSEvent = await _context.SSEvents
                .FirstOrDefaultAsync(m => m.SSEventId == id);
            if (sSEvent == null)
            {
                return NotFound();
            }

            return View(sSEvent);
        }

        // POST: SSEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDeleteEventConfirmed(Guid id)
        {
            var sSEvent = await _context.SSEvents.FindAsync(id);
            _context.SSEvents.Remove(sSEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminEventManagement));
        }

        private bool SSEventExists(Guid id)
        {
            return _context.SSEvents.Any(e => e.SSEventId == id);
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