using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.ViewModels;
using System.Text.Json;

using System.Text.Json;

namespace StudentRegisteration.Controllers;


public class StudentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IAddressService _addressService;
    private readonly IStudentDetailsService _studentDetailsService;
    private readonly IWebHostEnvironment _env;

    public StudentController(IWebHostEnvironment env,IStudentDetailsService studentDetailsService ,ApplicationDbContext context, IAddressService addressService)
    {
       
        _context = context;
        _addressService=addressService;
        _studentDetailsService= studentDetailsService;
        _env = env;
        
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

    [HttpGet]
    public IActionResult GetCitiesByState(string state)
    {
        var cities = _addressService.GetCitiesByStateAsync(state).Result;
        return Json(cities);
    }

    [HttpGet]
    public IActionResult GetTownByCity(string city)
    {
        var towns = _addressService.GetTownByCityAsync(city).Result;
        return Json(towns);
    }

    public  async Task<IActionResult> Index()
    {
        if(!IsUserInRole("Student"))
        {
            return RedirectToAction("Login", "User");
        }

        var loggedInUser = GetLoggedInUser();
            if (loggedInUser == null)
            {
                return NotFound("No loggedInUser data found");
            }
        
        var studentDetails = await _context.StudentDetails.FirstOrDefaultAsync(sd => sd.UserId == loggedInUser.Id);
        if(studentDetails == null)
        {
            return View(studentDetails);
        }
        else
        {
            return RedirectToAction("Dashboard", new {id = studentDetails.Id});
        }
        // return View(Dashboard);
              
    }

    [HttpGet]
    public async Task<IActionResult> Create(string state = "", string city = "", string town = "")
    {
        var viewModel = new StudentDetailsCreateViewModel
        {
 
            States = await _addressService.GetDistinctStatesAsync(),
            Cities = await _addressService.GetCitiesByStateAsync(state),
            Towns = await _addressService.GetTownByCityAsync(city),

            State = state,
            City = city,
            Town = town
        };

        if (string.IsNullOrEmpty(viewModel.NrcFrontPicPath))
        {
            ModelState.MarkFieldValid("NrcFrontPicPath");
        }

        if (string.IsNullOrEmpty(viewModel.NrcBackPicPath))
        {
            ModelState.MarkFieldValid("NrcBackPicPath");
        }

        if (string.IsNullOrEmpty(viewModel.ProfilePicPath))
        {
            ModelState.MarkFieldValid("ProfilePicPath");
        }

        var loggedInUser = GetLoggedInUser();
        if (loggedInUser != null)
        {
            var studentDetails = await _studentDetailsService.GetStudentDetailsByUserIdAsync(loggedInUser.Id);
            if (studentDetails != null)
            {
                viewModel.Id = studentDetails.Id;
                viewModel.Name = studentDetails.Name;
                viewModel.PhoneNumber = studentDetails.PhoneNumber;
                viewModel.NrcNumber = studentDetails.NrcNumber;
                viewModel.NrcFrontPicPath = studentDetails.NrcFrontPicPath;
                viewModel.NrcBackPicPath = studentDetails.NrcBackPicPath;
                viewModel.ProfilePicPath = studentDetails.ProfilePicPath;
                viewModel.UserId = studentDetails.UserId;
                viewModel.Enrollments = studentDetails.Enrollments;
                viewModel.RegistrationNumber = studentDetails.RegistrationNumber;
                viewModel.RegistrationDate = studentDetails.RegistrationDate;

                var addressDetails = await _addressService.GetAddressDetailsByAddressIdAsync(studentDetails.AddressId.Value);
                if (addressDetails != null)
                {
                    viewModel.State = addressDetails.State;
                    viewModel.City = addressDetails.City;
                    viewModel.Town = addressDetails.Town;
                }
            }
        }
       
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentDetailsCreateViewModel viewModel,
                        IFormFile nrcFrontPic, IFormFile nrcBackPic, IFormFile profilePic)
    { 
        var loggedInUser = GetLoggedInUser();
        if (loggedInUser == null)
        {
            return View(viewModel);
        }
            
        int? addressId = await _addressService.FindAddressIdByFiltersAsync(viewModel.State, viewModel.City, viewModel.Town);

        if (addressId == null)
        {
            ModelState.AddModelError("State", "Invalid State");
            ModelState.AddModelError("City", "Invalid City");
            ModelState.AddModelError("Town", "Invalid Town");

            viewModel.States = await _addressService.GetDistinctStatesAsync();
            viewModel.Cities = await _addressService.GetCitiesByStateAsync(viewModel.State);
            viewModel.Towns = await _addressService.GetTownByCityAsync(viewModel.City);

            return View(viewModel);
        }
        var studentDetails = await _studentDetailsService.GetStudentDetailsByUserIdAsync(loggedInUser.Id);

        string nrcFrontPicPath = null;
        string nrcBackPicPath = null;
        string profilePicPath = null;

        if (nrcFrontPic == null || nrcFrontPic.Length == 0 )
        {
                ModelState.AddModelError("NrcFrontPicPath", "Please upload a valid NRC Front Pic");
        }
        else
        {
            nrcFrontPicPath = await _studentDetailsService.SaveFileAsync(nrcFrontPic, _env.WebRootPath);
        }

        if (nrcBackPic == null || nrcBackPic.Length == 0 )
        {
            ModelState.AddModelError("NrcBackPicPath", "Please upload a valid NRC Back Pic");
        }
        else
        {
            nrcBackPicPath = await _studentDetailsService.SaveFileAsync(nrcBackPic, _env.WebRootPath);
        }

        if (profilePic == null || profilePic.Length == 0 )
        {
            ModelState.AddModelError("ProfilePicPath", "Please upload a valid Profile Pic");
        }
        else
        {
            profilePicPath = await _studentDetailsService.SaveFileAsync(profilePic, _env.WebRootPath);
        }

       
        if(!ModelState.IsValid)
        { 
            if(studentDetails == null)
            {
                studentDetails = new StudentDetails
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = viewModel.Name,
                    PhoneNumber = viewModel.PhoneNumber,

            // gonna use with Custom Validation 
                    NrcNumber = viewModel.NrcNumber,

            // gonna Use with Custom Private Method
                    RegistrationNumber = "STU" + DateTime.Now.ToString("yyMMddHHmmss"),
                    RegistrationDate =  DateTime.Now,

                    UserId = loggedInUser.Id,
                    AddressId = addressId.Value,
                    Enrollments = viewModel.Enrollments,

            //Mapping Image paths based on uploaded files
                    NrcFrontPicPath = nrcFrontPicPath,
                    NrcBackPicPath = nrcBackPicPath,
                    ProfilePicPath = profilePicPath,
                };

                try 
                {
                    await _studentDetailsService.AddStudentDetailsAsync(studentDetails, nrcFrontPic, nrcBackPic, profilePic);
                   
                        return RedirectToAction("Dashboard", new {id = studentDetails.Id});
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ModelState.AddModelError("", "An error occurred while creating student details.");

                    viewModel.States = await _addressService.GetDistinctStatesAsync();
                    viewModel.Cities = await _addressService.GetCitiesByStateAsync(viewModel.State);
                    viewModel.Towns = await _addressService.GetTownByCityAsync(viewModel.City);
                            
                    return View(viewModel);
                }
                
            }
            else
            {
                viewModel.Id = studentDetails.Id;
                viewModel.Name = studentDetails.Name;
                viewModel.PhoneNumber = studentDetails.PhoneNumber;
                studentDetails.NrcNumber = viewModel.NrcNumber;
                viewModel.NrcFrontPicPath = studentDetails.NrcFrontPicPath;
                viewModel.NrcBackPicPath = studentDetails.NrcBackPicPath;
                viewModel.ProfilePicPath = studentDetails.ProfilePicPath;
                viewModel.UserId = studentDetails.UserId;
                viewModel.Enrollments = studentDetails.Enrollments;
                viewModel.RegistrationNumber = studentDetails.RegistrationNumber;
                viewModel.RegistrationDate = studentDetails.RegistrationDate;
                studentDetails.AddressId = addressId.Value;

                try
                {
                    await _studentDetailsService.UpdateStudentDetailsAsync(studentDetails, nrcFrontPic, nrcBackPic, profilePic);
                    return RedirectToAction("Dashboard", new {id = studentDetails.Id});
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ModelState.AddModelError("", "An error occurred while creating student details.");
    
                    viewModel.States = await _addressService.GetDistinctStatesAsync();
                    viewModel.Cities = await _addressService.GetCitiesByStateAsync(viewModel.State);
                    viewModel.Towns = await _addressService.GetTownByCityAsync(viewModel.City);
                    
                    return View(viewModel);
                }
                
            }

        }
        return View(viewModel);
    }

    public async Task<IActionResult> Dashboard(string id)
    {
        if (id == null)
        {
            return NotFound("No ID found!");
        }
        var studentDetails = await _studentDetailsService.GetStudentDetailsByIdAsync(id);
        if (studentDetails == null)
        {
            return NotFound("No StudentDetails with that ID");
        }

        var addressDetails = await _addressService.GetAddressDetailsByAddressIdAsync(studentDetails.AddressId.Value);
        if(addressDetails == null)
        {
            return NotFound("No address Details with that ID");
        }

        var viewModel = new StudentDetailsViewModel
        {
            Id = studentDetails.Id,
            Name = studentDetails.Name,
            PhoneNumber = studentDetails.PhoneNumber,
            NrcNumber = studentDetails.NrcNumber,

            RegistrationNumber = studentDetails.RegistrationNumber,
            RegistrationDate = studentDetails.RegistrationDate,

            State = addressDetails.State,
            City = addressDetails.City,
            Town = addressDetails.Town,

            NrcFrontPicPath = studentDetails.NrcFrontPicPath,
            NrcBackPicPath = studentDetails.NrcBackPicPath,
            ProfilePicPath = studentDetails.ProfilePicPath,
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Details (string id)
    {
        if(id == null)
        {
            return NotFound("No Id Found");
        }

        var studentDetails = await _studentDetailsService.GetStudentDetailsByIdAsync(id);
        if (studentDetails == null )
        {
            return NotFound("no Student Details that is Related to id");
        }

        var addressDetails = await _addressService.GetAddressDetailsByAddressIdAsync(studentDetails.AddressId.Value);
        if(addressDetails == null)
        {
            return NotFound("No address Details with that ID");
        }

        var viewModel = new StudentDetailsViewModel
        {
            Id = studentDetails.Id,
            Name = studentDetails.Name,
            PhoneNumber = studentDetails.PhoneNumber,
            NrcNumber = studentDetails.NrcNumber,

            RegistrationNumber = studentDetails.RegistrationNumber,
            RegistrationDate = studentDetails.RegistrationDate,

            State = addressDetails.State,
            City = addressDetails.City,
            Town = addressDetails.Town,

            NrcFrontPicPath = studentDetails.NrcFrontPicPath,
            NrcBackPicPath = studentDetails.NrcBackPicPath,
            ProfilePicPath = studentDetails.ProfilePicPath,
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if(id == null)
        {
            return NotFound("No Id Found!");
        }
        
        var studentDetails = await _studentDetailsService.GetStudentDetailsByIdAsync(id);

        if(studentDetails == null )
        {
            return NotFound("No student details with that ID");
        }

        try
        {
            await _studentDetailsService.DeleteStudentDetails(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting student details: {ex.Message}");
            ModelState.AddModelError("", "An error occurred while deleting student details. Please try again.");
            return RedirectToAction("Index");
        }
    }

}