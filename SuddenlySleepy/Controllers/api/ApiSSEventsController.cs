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
    public class ApiSSEventsController : ControllerBase
    {
        private readonly AppIdentityDbContext _context;

        public ApiSSEventsController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: api/ApiSSEventsController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SSEvent>>> GetSSEvents()
        {
            return await _context.SSEvents.ToListAsync();
        }

        // GET: api/ApiSSEventsController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SSEvent>> GetSSEvent(Guid id)
        {
            var sSEvent = await _context.SSEvents.FindAsync(id);

            if (sSEvent == null)
            {
                return NotFound();
            }

            return sSEvent;
        }

        // PUT: api/ApiSSEventsController/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSSEvent(Guid id, SSEvent sSEvent)
        {
            if (id != sSEvent.SSEventId)
            {
                return BadRequest();
            }

            _context.Entry(sSEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SSEventExists(id))
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

        // POST: api/ApiSSEventsController
        [HttpPost]
        public async Task<ActionResult<SSEvent>> PostSSEvent(SSEvent sSEvent)
        {
            _context.SSEvents.Add(sSEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSSEvent", new { id = sSEvent.SSEventId }, sSEvent);
        }

        // PATCH: api/ApiSSEventsController/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<SSEvent>> PatchSSEvent(Guid id,
            [FromBody]JsonPatchDocument<SSEvent> patch)
        {
            SSEvent sSEvent = _context.SSEvents.Find(id);
            if (sSEvent != null)
            {
                patch.ApplyTo(sSEvent);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        // DELETE: api/ApiSSEventsController/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SSEvent>> DeleteSSEvent(Guid id)
        {
            var sSEvent = await _context.SSEvents.FindAsync(id);
            if (sSEvent == null)
            {
                return NotFound();
            }

            _context.SSEvents.Remove(sSEvent);
            await _context.SaveChangesAsync();

            return sSEvent;
        }

        private bool SSEventExists(Guid id)
        {
            return _context.SSEvents.Any(e => e.SSEventId == id);
        }
    }
}
