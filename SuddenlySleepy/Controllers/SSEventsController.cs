using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuddenlySleepy.Models;

namespace SuddenlySleepy.Controllers
{
    public class SSEventsController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public SSEventsController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: SSEvents
        public async Task<IActionResult> Index()
        {
            return View(await _context.SSEvents.ToListAsync());
        }

        // GET: SSEvents/Details/5
        public async Task<IActionResult> Details(Guid? id)
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

        // GET: SSEvents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SSEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SSEventId,MeetingDate,Location,SSEventName,Description")] SSEvent sSEvent)
        {
            if (ModelState.IsValid)
            {
                sSEvent.SSEventId = Guid.NewGuid();
                _context.Add(sSEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sSEvent);
        }

        // GET: SSEvents/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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
        public async Task<IActionResult> Edit(Guid id, [Bind("SSEventId,MeetingDate,Location,SSEventName,Description")] SSEvent sSEvent)
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
                return RedirectToAction(nameof(Index));
            }
            return View(sSEvent);
        }

        // GET: SSEvents/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sSEvent = await _context.SSEvents.FindAsync(id);
            _context.SSEvents.Remove(sSEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SSEventExists(Guid id)
        {
            return _context.SSEvents.Any(e => e.SSEventId == id);
        }
    }
}
