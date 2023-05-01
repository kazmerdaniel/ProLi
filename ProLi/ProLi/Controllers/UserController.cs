using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProLi.Data;
using ProLi.Models;


public class ApplicationUser
{
    public string Email { get; set; }
}

public class UserController : Controller
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    private readonly ApplicationDbContext _context;

    public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _context = context;
        _roleManager = roleManager;
    }


    public IActionResult Index()
    {
        return View();
    }

   
    public async Task<ViewResult> GetUsers()
    {
        IdentityUser applicationUser = await _userManager.GetUserAsync(User);

        // For ASP.NET Core <= 3.1
        var users = await _userManager.Users.ToListAsync();
        ViewData["Title"] = "Felhasználók";

        return View(users);

    }
   
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null || _context.People == null)
        {
            return NotFound();
        }

        var people = await _userManager.FindByIdAsync(id);
        if (people == null)
        {
            return NotFound();
        }
        System.Diagnostics.Debug.WriteLine(people);
        return View(people);
    }

    
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null || _context.People == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);


        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count != 0)
        {
            var userRole = "";
            switch (roles.ElementAt(0))
            {
                case "Administrator":
                    userRole = "Adminisztrátor";    
                        break;
                case "BigBoss":
                    userRole = "Felső vezető";
                    break;
                case "System":
                    userRole = "Rendszergazda";
                    break;
            }
            ViewData["userRole"] = userRole;
        }
        else
        {
            ViewData["userRole"] = "Felhasználó";
        }



        return View(user);
    }


    [Authorize(Roles = "System,Administrator")]
    [HttpGet]
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null || _context.People == null)
        {
            return NotFound();
        }
        var user = await _userManager.FindByIdAsync(id);
        ViewData["name"] = user.UserName;
         
        return View(user);
    }


    [Authorize(Roles = "System,Administrator")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        if (id == null || _context.People == null)
        {
            return NotFound();
        }
        var user = await _userManager.FindByIdAsync(id);
        await _userManager.DeleteAsync(user);

        return RedirectToAction("GetUsers");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, [Bind("Id,Email,UserName,PhoneNumber")] IdentityUser user)
        {
            ViewData["id"] = id;        
            if (id != user.Id || user.Id == null)
            {
                return NotFound();
            }

        
            var usR = await _userManager.FindByIdAsync(user.Id);
            usR.Email = user.Email;
            usR.UserName = user.UserName;
            usR.PhoneNumber = user.PhoneNumber;


        await _userManager.UpdateAsync(usR);
        return RedirectToAction("GetUsers");
    }

    [HttpPost]
    public async Task<IActionResult> Create ([Bind("Email,UserName,PhoneNumber,Password,UserRole")] IdentityUserWithRole user)
    {
        var newUser = new IdentityUser();
        newUser.Email = user.Email;
        newUser.UserName = user.UserName;
        newUser.PhoneNumber = user.PhoneNumber;
        newUser.EmailConfirmed = true;
        newUser.PhoneNumberConfirmed = true;

        var usR = await _userManager.CreateAsync(newUser, user.Password);
        if (usR.Succeeded)
        {
            var newUserCreated = await _userManager.FindByEmailAsync(user.Email);
            bool x = await _roleManager.RoleExistsAsync(user.UserRole);
            if (x && user.UserRole ! == null)
            {
                await _userManager.AddToRoleAsync(newUserCreated, user.UserRole);
            }
            else
            {
                var roleToUse = user.UserRole;

                var newRole = new IdentityRole();
                newRole.Name = roleToUse;
                newRole.NormalizedName = roleToUse;
                var roleCreationResult = await _roleManager.CreateAsync(newRole);
                if(roleCreationResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUserCreated, roleToUse);

                }

            }

        }


      

        return RedirectToAction("GetUsers");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
   
        return View();
    }

    private bool UserExists(int id)
    {
        throw new NotImplementedException();
    }
}