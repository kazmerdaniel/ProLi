using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProLi.Models;

namespace ProLi.Controllers
{
    public class ProlieventsController : Controller
    {
        private readonly ProlidbContext _context;

        public ProlieventsController(ProlidbContext context)
        {
            _context = context;
        }

        // GET: Prolievents
        public async Task<IActionResult> Index(string searchString)
        {
              ////  Original:
              //return _context.Prolievents != null ? 
              //            View(await _context.Prolievents.ToListAsync()) :
              //            Problem("Entity set 'ProlidbContext.Prolievents'  is null.");


            //Lista szűkítése a névre szűrve

            if (_context.Prolievents == null)
            {
                return Problem("Entity set 'ProlidbContext.Prolievents'  is null.");
            }
            string filter = "%" + searchString + "%";
            var eventname = _context.Prolievents.Where(c => EF.Functions.Like(c.EventName, filter)).ToList();

            if (filter == "")
            {
                return View(await _context.Prolievents.ToListAsync());
            }
            if (!String.IsNullOrEmpty(filter))
            {
                return View(eventname);
            }

            return View(await _context.Prolievents.ToListAsync());




        }

        // GET: Prolievents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prolievents == null)
            {
                return NotFound();
            }

            var prolievent = await _context.Prolievents

                .FirstOrDefaultAsync(m => m.EventId == id);
            if (prolievent == null)
            {
                return NotFound();
            }

            return View(prolievent);


                        
        }






        // GET: Prolievents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prolievents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDate,EventPlace,EventHead,EventStatus")] Prolievent prolievent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prolievent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prolievent);
        }

        // GET: Prolievents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prolievents == null)
            {
                return NotFound();
            }

            var prolievent = await _context.Prolievents.FindAsync(id);
            if (prolievent == null)
            {
                return NotFound();
            }
            return View(prolievent);
        }

        // POST: Prolievents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,EventPlace,EventHead,EventStatus")] Prolievent prolievent)
        {
            if (id != prolievent.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prolievent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProlieventExists(prolievent.EventId))
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
            return View(prolievent);
        }

        // GET: Prolievents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prolievents == null)
            {
                return NotFound();
            }

            var prolievent = await _context.Prolievents
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (prolievent == null)
            {
                return NotFound();
            }

            return View(prolievent);
        }

        // POST: Prolievents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prolievents == null)
            {
                return Problem("Entity set 'ProlidbContext.Prolievents'  is null.");
            }
            var prolievent = await _context.Prolievents.FindAsync(id);
            if (prolievent != null)
            {
                _context.Prolievents.Remove(prolievent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProlieventExists(int id)
        {
          return (_context.Prolievents?.Any(e => e.EventId == id)).GetValueOrDefault();
        }
    }
}
