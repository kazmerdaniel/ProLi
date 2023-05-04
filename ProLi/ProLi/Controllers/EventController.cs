using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using ProLi.Data;
using ProLi.Models;

namespace ProLi.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<EventPeople> eventPeoples = new List<EventPeople>();     //mehívások


        public EventController(ApplicationDbContext context)
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
            if (_context.Event == null)
            {
                return Problem("Entity set 'ProlidbContext.Prolievents'  is null.");
            }
            string filter = "%" + searchString + "%";
            var eventname = _context.Event.Where(c => EF.Functions.Like(c.EventName, filter)).ToList();

            if (filter == "")
            {
                return View(await _context.Event.ToListAsync());
            }
            if (!String.IsNullOrEmpty(filter))
            {
                return View(eventname);
            }

            return View(await _context.Event.ToListAsync());
        }
        // GET: People/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // GET: People/ShowSearchResult
        public async Task<IActionResult> ShowSearchResults(String QueryString)
        {
            var query = CreateSearchQuery(_context.Event, QueryString);

            var result = await query.ToListAsync();

            return View(result);

        }
        // GET: People/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Event == null)
            {
                return NotFound();
            }
            var @event = await _context.Event
            .Include(e => e.People)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }
        [Authorize(Roles = "System,Administrator")]
        public async Task<IActionResult> DeletePerson(int? id, int? personId)
        {
            if (id == null || _context.Event == null)
            {
                return NotFound();
            }
            var @event = await _context.Event
            .Include(e => e.People)
                .FirstOrDefaultAsync(e => e.Id == id);


            if (@event != null)
            {
                string connectionString = "Server=prolidbserverr.mysql.database.azure.com;Database=prolidb;Port=3306;User Id=proliadmin;Password=Milton2022;SSL Mode=Required";
                MySqlConnection connection = new MySqlConnection(connectionString);

                connection.Open();

                // Create a SQL command to execute
                string sql = "DELETE FROM EventPeople WHERE EventsId = " + id + " AND PeopleId = " + personId + ";";
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Do something with the results
                    Console.WriteLine(reader.GetString(0));
                }
            }
            return RedirectToAction("Details", new { id = id });

        }
      
        // GET: People/Create
        public async Task<IActionResult> Create()
        {
            List<People> userList = _context.People.ToList();
            ViewBag.ShowMembers = new SelectList(userList, "Id", "GuestName");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddToEvent(int? id)
        {
            var people = await _context.People.ToListAsync();
            var @event = await _context.Event
             .Include(e => e.People)
            .FirstOrDefaultAsync(e => e.Id == id);
            var peoplee = @event.People;
            var filtered = people
                               .Where(x => !peoplee.Any(y => y.Id == x.Id));

            if (people != null)
            {
                ViewData["people"] = people;
            }
            return View(filtered);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToEvent(int? id, int person)
        {

            // a kiválasztott személy hozzárendelése a kiválasztott adatbázishoz
            if (id == null || _context.Event == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            var guy = await _context.People.FindAsync(person);

            if (@event != null && guy != null)
            {
                @event.People.Add(guy);
                _context.Update(@event);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));

        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventName,EventTime,Place,MaxPeople")] Event ev)
        {
            _context.Add(ev);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Event == null)
            {
                return NotFound();
            }

            var people = await _context.Event.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventName,EventTime,Place,MaxPeople")] Event people)
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

        }
        [Authorize(Roles = "System,Administrator")]
        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            System.Diagnostics.Debug.WriteLine("22222");
            if (id == null || _context.Event == null)
            {
                return NotFound();
            }

            var people = await _context.Event
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
            var people = await _context.Event.FindAsync(id);
            if (people != null)
            {
                System.Diagnostics.Debug.WriteLine("people törls");
                _context.Event.Remove(people);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeopleExists(int id)
        {
            return (_context.Event?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        //Adott rendezvény résztvevőinek listája exportálás Excelbe
           

        [HttpPost, ActionName("ExportToExcel1")]
        public IActionResult ExportToExcel1(int id)
        {
            using (var workbook = new XLWorkbook())
            {
                var eventDetails = _context.Event.Include(e => e.People).FirstOrDefault(e => e.Id == id);
                if (eventDetails == null)
                {
                    return NotFound();
                }

                var worksheet = workbook.Worksheets.Add("Meghívottak");
                var currentRow = 6;

                worksheet.Cell(1, 1).Value = eventDetails.EventName;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(2, 1).Value = eventDetails.EventTime;
                worksheet.Cell(2, 1).Style.Font.Bold = true;
                worksheet.Cell(3, 1).Value = eventDetails.Place;
                worksheet.Cell(3, 1).Style.Font.Bold = true;
                worksheet.Cell(5, 1).Value = "Résztvevők:";
                worksheet.Cell(5, 1).Style.Font.Bold = true;

                worksheet.Range("A6:F6").Style.Fill.BackgroundColor = XLColor.LightGray;
                worksheet.Range("A1:F4").Style.Font.FontSize = 12;

                worksheet.Cell(currentRow, 1).Value = "Név";
                worksheet.Cell(currentRow, 2).Value = "e-mail cím";
                worksheet.Cell(currentRow, 3).Value = "Cím";
                worksheet.Cell(currentRow, 4).Value = "Telefon";
                worksheet.Cell(currentRow, 5).Value = "Ország";
                worksheet.Cell(currentRow, 6).Value = "Megjegyzés";

                foreach (var people in eventDetails.People)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = people.GuestName;
                    worksheet.Cell(currentRow, 2).Value = people.Email;
                    worksheet.Cell(currentRow, 3).Value = people.Address;
                    worksheet.Cell(currentRow, 4).Value = people.Phone;
                    worksheet.Cell(currentRow, 5).Value = people.Country;
                    worksheet.Cell(currentRow, 6).Value = people.Note;
                }

                worksheet.Range("A5:F" + currentRow).Style.Font.FontSize = 12;
                worksheet.Range("A5:F5").Style.Font.Bold = true;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{eventDetails.EventName} résztvevők listája.xlsx");
                }
            }
        }
    }  
}
