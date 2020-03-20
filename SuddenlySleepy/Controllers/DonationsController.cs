using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuddenlySleepy.Models;
using Microsoft.AspNetCore.Authorization;


namespace SuddenlySleepy
{
    public class DonationsController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public DonationsController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: Donations
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

            var donation = await _context.Donations
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationId,DonationAmount")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                donation.DonationId = Guid.NewGuid();
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
