using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.Services;
using StudentRegisteration.ViewModels;

namespace StudentRegisteration.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
   
 
    public AdminController(ApplicationDbContext context)
    {
        _context = context;
         
    }

    private bool IsUserInRole(string role)
    {
        var loggedInUserString = HttpContext.Session.GetString("loggedInUser");
        if(loggedInUserString == null)
        {
            return false;
        }

        // User loggedInUser;
        try 
        {
            var loggedInUser = JsonSerializer.Deserialize<User>(loggedInUserString);
            return loggedInUser.Role == role;
        }
        catch(JsonException ex)
        {
                // Log the exception
            Console.WriteLine($"Error deserializing user data: {ex.Message}");

            return false;
        }          
    }

    private User GetLoggedInUser()
    {
        var loggedInUserString = HttpContext.Session.GetString("loggedInUser");

        if (loggedInUserString != null)
        {
            try
            {
                var loggedInUser = JsonSerializer.Deserialize<User>(loggedInUserString);
                return loggedInUser;
            }
            catch(JsonException ex)
            {
                Console.WriteLine($"Error deserializing user data: {ex.Message}");

                return null;
            }
        }
        else
        {
            return null;
        }
    }

    public IActionResult Index()
    {

        if(!IsUserInRole("Admin"))
        {
            return RedirectToAction("Login","User");
        }

        return RedirectToAction("Dashboard");
    }

    public IActionResult Dashboard(AdminViewModel viewModel)
    {
   
        if(!IsUserInRole("Admin"))
        {
            return RedirectToAction("Login","User");
        }

        var admin = GetLoggedInUser();
        viewModel = new AdminViewModel
        {
            Id = admin.Id,
            UserName = admin.UserName,
            Email = admin.Email,
            Password = admin.Password
        };

        return View(viewModel);
    }

    public  IActionResult AddressManagement()
    {
        return RedirectToAction("Index","AddressManagement");
    }

    public IActionResult Address()
    {
        
        if(!IsUserInRole("Admin"))
        {
            return RedirectToAction("Login","User");
        }

        return RedirectToAction("Index","Address");
    }


     public async Task<IActionResult> CoursesManagement()
    {
        if(!IsUserInRole("Admin"))
        {
            return RedirectToAction("Login","User");
        }

        return RedirectToAction("Index","CourseOffering");
    }
    
    public async Task<IActionResult> StudentManagement()
    {
        if(!IsUserInRole("Admin"))
        {
            return RedirectToAction("Login","User");
        }

        return RedirectToAction("StudentAndEnrollmentList", "Enrollment");
    }

      public async Task<IActionResult> StudentCount()
    {
        if(!IsUserInRole("Admin"))
        {
            return RedirectToAction("Login","User");
        }

        return RedirectToAction("EnrollmentCounts", "Enrollment");
    }

    public async Task<IActionResult> MonthlyPaymentSheet()
    {
        if (!IsUserInRole("Admin"))
        {
            return RedirectToAction("Login", "User");
        }

        return RedirectToAction("MonthlyPaymentSheet", "Enrollment");
    }

    public ActionResult MarkList()
    {
        if (!IsUserInRole("Admin"))
        {
            return RedirectToAction("Login", "User");
        }

        return RedirectToAction("MarkList", "Enrollment");
    }
   
}