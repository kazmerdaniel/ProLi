using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProLi.Models;

namespace ProLi.Controllers
{
    public class ProlipersonsController : Controller
    {
        private readonly ProlidbContext _context;

   
        public ProlipersonsController(ProlidbContext context)
        {
            _context = context;
        }

        // GET: Prolipersons
        public async Task<IActionResult> Index(string searchString)
        {
            //Lista szűkítése a névre szűrve

            if (_context.Prolipeople == null)
            {
                return Problem("Entity set 'ProlidbContext.Prolipeople'  is null.");
            }
            string filter ="%"+searchString+"%";
            var person = _context.Prolipeople.Where(c => EF.Functions.Like(c.PersonName, filter)).ToList();

            if (filter=="")
            {
                return View(await _context.Prolipeople.ToListAsync());
            }
            if (!String.IsNullOrEmpty( filter))
            {
                return View(person);
            }        

                return View(await _context.Prolipeople.ToListAsync());            

        }

        // GET: Prolipersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {    
            if (id == null || _context.Prolipeople == null)
            {
                return NotFound();
            }

            var proliperson = await _context.Prolipeople
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (proliperson == null)
            {
                return NotFound();
            }

            return View(proliperson);
        }

        // GET: Prolipersons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prolipersons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,PersonName,PersonCountry,PersonEmail,PersonPhone,PersonComment,PersonSpecComment,PersonStatus")] Proliperson proliperson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proliperson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proliperson);
        }

        // GET: Prolipersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prolipeople == null)
            {
                return NotFound();
            }

            var proliperson = await _context.Prolipeople.FindAsync(id);
            if (proliperson == null)
            {
                return NotFound();
            }
            return View(proliperson);
        }

        // POST: Prolipersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,PersonName,PersonCountry,PersonEmail,PersonPhone,PersonComment,PersonSpecComment,PersonStatus")] Proliperson proliperson)
        {
            if (id != proliperson.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proliperson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProlipersonExists(proliperson.PersonId))
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
            return View(proliperson);
        }

        // GET: Prolipersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prolipeople == null)
            {
                return NotFound();
            }

            var proliperson = await _context.Prolipeople
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (proliperson == null)
            {
                return NotFound();
            }

            return View(proliperson);
        }

        // POST: Prolipersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prolipeople == null)
            {
                return Problem("Entity set 'ProlidbContext.Prolipeople'  is null.");
            }
            var proliperson = await _context.Prolipeople.FindAsync(id);
            if (proliperson != null)
            {
                _context.Prolipeople.Remove(proliperson);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProlipersonExists(int id)
        {
          return (_context.Prolipeople?.Any(e => e.PersonId == id)).GetValueOrDefault();
        }

    }
}
