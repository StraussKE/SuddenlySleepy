/// Site User interactive controls

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuddenlySleepy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SuddenlySleepy.Controllers;

namespace SuddenlySleepy.Controllers
{
    [Authorize]
    public class SSUserActionsController : Controller
    {
        private readonly AppIdentityDbContext _context;
        private UserManager<SSUser> _userManager;

        public SSUserActionsController(AppIdentityDbContext context, UserManager<SSUser> usrMgr)
        {
            _context = context;
            _userManager = usrMgr;
        }

        /// <summary>
        /// Lists donations for the member who is currently logged in
        /// </summary>
        /// Returns list view
        /// <returns></returns>
        public async Task<IActionResult> DonationHistory(string orderBy = "default")
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var currentUserDonations = _context.Donations.Where(donation => donation.Donor.Id == user.Id); // pulls current user log info from database

                if (orderBy == "amount")
                {
                    currentUserDonations = currentUserDonations.OrderBy(d => d.DonationAmount);
                }
                else
                {
                    currentUserDonations = currentUserDonations.OrderBy(d => d.DonationDate);
                }
                return View(currentUserDonations.ToList());
            }
            else
            {
                return View();
            }
        }

        // gets current user from login cookie
        private Task<SSUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // Accesses form to create a new donation
        // Accessible at all levels
        // GET: SSUserActions/Donate
        [AllowAnonymous]
        public IActionResult Donate()
        {
            return View();
        }

        // POST: SSUserActions/Donate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donate([Bind("DonationId,DonationAmount")] Donation donation)
        {
            var currentDonor = await GetCurrentUserAsync();
            donation.Donor = currentDonor;
            if (ModelState.IsValid)
            {
                donation.DonationId = Guid.NewGuid();
                donation.DonationDate = DateTime.Now;
                _context.Add(donation);
                await _context.SaveChangesAsync();
                if (donation.Donor == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (donation.Donor.UserName == "Admin")
                {
                    return RedirectToAction("Index", "Donations");
                }
                else
                {
                    return RedirectToAction("DonationHistory");
                }
            }
            return View(donation);
        }
    }
}