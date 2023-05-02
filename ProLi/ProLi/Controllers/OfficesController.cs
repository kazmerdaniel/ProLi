using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProLi.Data;
using ProLi.Models;

namespace ProLi.Controllers
{
    public class OfficesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfficesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Offices
        public async Task<IActionResult> Index()
        {
            //return _context.Office != null ? 
            //            View(await _context.Office.ToListAsync()) :
            //            Problem("Entity set 'ApplicationDbContext.Office'  is null.");
            var applicationDbContext = _context.Office.Include(o => o.People);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Offices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Office == null)
            {
                return NotFound();
            }

            var office = await _context.Office
                    .Include(o => o.People)
                    .FirstOrDefaultAsync(m => m.Id == id);
            if (office == null)
            {
                return NotFound();
            }

            return View(office);
        }

        // GET: Offices/Create
        public IActionResult Create()
        {
            ViewData["People_Id"] = new SelectList(_context.People, "Id", "Id");
            return View();
        }

        // POST: Offices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfficePost,OfficeStart,OfficeEnd,OfficeName1,OfficeName2,OfficeName3,OfficeAddress,OfficeEmail,OfficePhone,People_Id")] Office office)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(office);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(office);

            
            var count = _context.Office.Count();

            _context.Add(office);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        




        }

        // GET: Offices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Office == null)
            {
                return NotFound();
            }

            var office = await _context.Office.FindAsync(id);
            if (office == null)
            {
                return NotFound();
            }
            ViewData["People_Id"] = new SelectList(_context.People, "Id", "Id", office.People_Id);

            return View(office);
        }

        // POST: Offices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OfficePost,OfficeStart,OfficeEnd,OfficeName1,OfficeName2,OfficeName3,OfficeAddress,OfficeEmail,OfficePhone,People_Id")] Office office)
        {
            if (id != office.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        //_context.Update(office);
            //        //await _context.SaveChangesAsync();
            //        _context.Update(office);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));


            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!OfficeExists(office.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(office);
            try
            {
                _context.Update(office);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficeExists(office.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return View(office);

        }

        // GET: Offices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Office == null)
            {
                return NotFound();
            }

            var office = await _context.Office
                .FirstOrDefaultAsync(m => m.Id == id);
            if (office == null)
            {
                return NotFound();
            }

            return View(office);
        }

        // POST: Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Office == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Office'  is null.");
            }
            var office = await _context.Office.FindAsync(id);
            if (office != null)
            {
                _context.Office.Remove(office);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfficeExists(int id)
        {
          return (_context.Office?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
