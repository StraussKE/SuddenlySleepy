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

namespace SuddenlySleepy
{
    [Authorize] // only admin can access admin stuff
    public class DonationsController : Controller
    {
        private readonly AppIdentityDbContext _context;
        private UserManager<SSUser> _userManager;

        public DonationsController(AppIdentityDbContext context, UserManager<SSUser> usrMgr)
        {
            _context = context;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> MemberDonations()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                ViewBag.currentUserDonations = _context.Donations.Where(donation => donation.Donor.Id == user.Id).ToList(); // pulls current user log info from database
            }
            return View(user); // passes user into view
        }

        private Task<SSUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Donations
        [Authorize(Roles = "_Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Donations.ToListAsync());
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.Include(d => d.Donor)
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        [AllowAnonymous]
        // GET: Donations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationId,DonationAmount")] Donation donation)
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
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("MemberDonations");
                }
            }
            return View(donation);
        }

        // GET: Donations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: Donations/Delete/5
        [Authorize(Roles = "_Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var donation = await _context.Donations.FindAsync(id);
            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(Guid id)
        {
            return _context.Donations.Any(e => e.DonationId == id);
        }
    }
}
