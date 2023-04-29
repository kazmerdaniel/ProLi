using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProLi.Data;
using ProLi.Models;

namespace ProLi.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IQueryable<T> CreateSearchQuery<T>(DbSet<T> db_set, string value) where T : class
        {
            IQueryable<T> query = db_set;

            List<Expression> expressions = new List<Expression>();

            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");

            MethodInfo contains_method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            foreach (PropertyInfo prop in typeof(T).GetProperties().Where(x => x.PropertyType == typeof(string)))
            {
                MemberExpression member_expression = Expression.PropertyOrField(parameter, prop.Name);

                ConstantExpression value_expression = Expression.Constant(value, typeof(string));

                MethodCallExpression contains_expression = Expression.Call(member_expression, contains_method, value_expression);

                expressions.Add(contains_expression);
            }

            if (expressions.Count == 0)
                return query;

            Expression or_expression = expressions[0];

            for (int i = 1; i < expressions.Count; i++)
            {
                or_expression = Expression.OrElse(or_expression, expressions[i]);
            }

            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(
                or_expression, parameter);

            return query.Where(expression);
        }

        // GET: People
        public async Task<IActionResult> Index(string searchString)
        {
            //return _context.People != null ?
            //            View(await _context.People.ToListAsync()) :
            //            Problem("Entity set 'ApplicationDbContext.People'  is null.");

            //Lista szűkítése a névre szűrve

            if (_context.People == null)
            {
                return Problem("Entity set 'ProlidbContext.Prolipeople'  is null.");
            }
            string filter = "%" + searchString + "%";
            var person = _context.People.Where(c => EF.Functions.Like(c.GuestName, filter)).ToList();

            if (filter == "")
            {
                return View(await _context.People.ToListAsync());
            }
            if (!String.IsNullOrEmpty(filter))
            {
                return View(person);
            }

            return View(await _context.People.ToListAsync());

        }
        // GET: People/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

  
        // GET: People/ShowSearchResult
        public async Task<IActionResult> ShowSearchResults(String QueryString)
        {
            var query = CreateSearchQuery(_context.People, QueryString);

            var result = await query.ToListAsync();

            return View(result);
            
        }
            // GET: People/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var people = await _context.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (people == null)
            {
                return NotFound();
            }

            return View(people);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

    

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GuestName,Title,Address,Organization,Email,Image,Country,Phone,SpecialNote,Note")] People people)
        {
            var count = _context.People.Count();
           
            _context.Add(people);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var people = await _context.People.FindAsync(id);
            if (people == null)
            {
                return NotFound();
            }
            return View(people);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GuestName,Title,Address,Organization,Email,Image,Country,Phone,SpecialNote,Note")] People people)
        {
            if (id != people.Id)
            {
                return NotFound();
            }

         
                try
                {
                    _context.Update(people);
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException)
                {
                    if (!PeopleExists(people.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                
            }
            return View(people);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var people = await _context.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (people == null)
            {
                return NotFound();
            }

            return View(people);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'ApplicationDbContext.People'  is null.");
            }
            var people = await _context.People.FindAsync(id);
            if (people != null)
            {
                _context.People.Remove(people);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeopleExists(int id)
        {
          return (_context.People?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
