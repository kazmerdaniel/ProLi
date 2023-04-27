using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProLi.Models;

namespace ProLi.Controllers
{
    public class ProliinvitesController : Controller
    {
        private readonly ProlidbContext _context;

        public ProliinvitesController(ProlidbContext context)
        {
            _context = context;
        }

        // GET: Proliinvites
        public async Task<IActionResult> Index(string Searchs)
        {
            ////Original
            //var prolidbContext = _context.Proliinvites.Include(p => p.Event).Include(p => p.Person);
            //return View(await prolidbContext.ToListAsync());

          
            var prolidbContext = _context.Proliinvites.Include(p => p.Event).Include(p => p.Person);
            return View(await prolidbContext.ToListAsync());

        }

        // GET: Proliinvites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proliinvites == null)
            {
                return NotFound();
            }

            var proliinvite = await _context.Proliinvites
                .Include(p => p.Event)
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.InviteId == id);
            if (proliinvite == null)
            {
                return NotFound();
            }

            return View(proliinvite);
        }

        // GET: Proliinvites/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Prolievents, "EventId", "EventId");
            ViewData["PersonId"] = new SelectList(_context.Prolipeople, "PersonId", "PersonId");
            return View();
        }

        // POST: Proliinvites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InviteId,EventId,PersonId")] Proliinvite proliinvite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proliinvite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Prolievents, "EventId", "EventId", proliinvite.EventId);
            ViewData["PersonId"] = new SelectList(_context.Prolipeople, "PersonId", "PersonId", proliinvite.PersonId);
            return View(proliinvite);
        }

        // GET: Proliinvites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proliinvites == null)
            {
                return NotFound();
            }

            var proliinvite = await _context.Proliinvites.FindAsync(id);
            if (proliinvite == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Prolievents, "EventId", "EventId", proliinvite.EventId);
            ViewData["PersonId"] = new SelectList(_context.Prolipeople, "PersonId", "PersonId", proliinvite.PersonId);
            return View(proliinvite);
        }

        // POST: Proliinvites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InviteId,EventId,PersonId")] Proliinvite proliinvite)
        {
            if (id != proliinvite.InviteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proliinvite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProliinviteExists(proliinvite.InviteId))
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
            ViewData["EventId"] = new SelectList(_context.Prolievents, "EventId", "EventId", proliinvite.EventId);
            ViewData["PersonId"] = new SelectList(_context.Prolipeople, "PersonId", "PersonId", proliinvite.PersonId);
            return View(proliinvite);
        }

        // GET: Proliinvites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proliinvites == null)
            {
                return NotFound();
            }

            var proliinvite = await _context.Proliinvites
                .Include(p => p.Event)
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.InviteId == id);
            if (proliinvite == null)
            {
                return NotFound();
            }

            return View(proliinvite);
        }

        // POST: Proliinvites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proliinvites == null)
            {
                return Problem("Entity set 'ProlidbContext.Proliinvites'  is null.");
            }
            var proliinvite = await _context.Proliinvites.FindAsync(id);
            if (proliinvite != null)
            {
                _context.Proliinvites.Remove(proliinvite);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProliinviteExists(int id)
        {
          return (_context.Proliinvites?.Any(e => e.InviteId == id)).GetValueOrDefault();
        }
    }
}
