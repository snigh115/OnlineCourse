
using System.Text;
using System.Text.Json;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.ViewModels;


namespace StudentRegisteration.Controllers;

public class UserController : Controller
{
    
    private readonly IUserService<User> _userService;

    public UserController(IUserService<User> userService)
    {
       
        _userService = userService;
       
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    //V2
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
            Password = model.Password,
            Role = model.SelectedRole,
            
        };

        var registrationResult = await _userService.Register(user);

        if(registrationResult.IsSuccessful)
        {
            return RedirectToAction("Login");
        }
        else
        {
            ViewData["ErrorMessage"] = registrationResult.ErrorMessage;
            return View(model);
        }

    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
       

        var loginResult = await _userService.Login(model.UsernameOrEmail,model.Password);

        if(loginResult.IsAuthenticated)
        {
            var loggedInUser = loginResult.User;
            HttpContext.Session.SetString("loggedInUser",JsonSerializer.Serialize(loggedInUser));

            
// check role 
            if(loggedInUser.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (loggedInUser.Role == "Student")
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                ModelState.AddModelError("","Invalid role for user");
                return View(model);
            }

        }
        else
        {
            ViewData["ErrorMessage"] = "Invalid  username or password";
            return View(model);
        }        
    }   

    public async Task<IActionResult> LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
} 

