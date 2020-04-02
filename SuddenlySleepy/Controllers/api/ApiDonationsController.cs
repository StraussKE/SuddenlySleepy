using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuddenlySleepy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;


namespace SuddenlySleepy.Controllers
{
    [Authorize(Roles = "_Admin")] // only admin can access API controls
    [Route("api/[controller]")]
    [ApiController]
    public class ApiDonationsController : ControllerBase
    {
        private readonly AppIdentityDbContext _context;

        public ApiDonationsController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: api/ApiDonations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonations()
        {
            return await _context.Donations.ToListAsync();
        }

        // GET: api/ApiDonations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonation(Guid id)
        {
            var donation = await _context.Donations.FindAsync(id);

            if (donation == null)
            {
                return NotFound();
            }

            return donation;
        }

        // PUT: api/ApiDonations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonation(Guid id, Donation donation)
        {
            if (id != donation.DonationId)
            {
                return BadRequest();
            }

            _context.Entry(donation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApiDonations
        [HttpPost]
        public async Task<ActionResult<Donation>> PostDonation(Donation donation)
        {
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonation", new { id = donation.DonationId }, donation);
        }

        // PATCH: api/ApiDonations/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Donation>> PatchLogEntry(Guid id,
            [FromBody]JsonPatchDocument<Donation> patch)
        {
            Donation donation = _context.Donations.Find(id);
            if (donation != null)
            {
                patch.ApplyTo(donation);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        // DELETE: api/ApiDonations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Donation>> DeleteDonation(Guid id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();

            return donation;
        }

        private bool DonationExists(Guid id)
        {
            return _context.Donations.Any(e => e.DonationId == id);
        }
    }
}
