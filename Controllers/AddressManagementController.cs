using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.ViewModels;

namespace StudentRegisteration.Controllers;

public class AddressManagementController : Controller
{
    // private readonly ApplicationDbContext _context;
    // private readonly IAddressService _addressService;

    // public AddressManagementController(ApplicationDbContext context, IAddressService addressService)
    // {
    //     _context = context;
    //     _addressService = addressService;
    // }

    //  private bool IsUserLoggedIn()
    // {
    //     var userId = HttpContext.Session.GetString("IsLoggedIn");
    //     return userId != null;
    // }

    // private bool IsAdmin()
    // {
    //     var userId = HttpContext.Session.GetString("IsLoggedIn");
    //     if (userId == null)
    //     {
    //         return false;
    //     }
    //     var user = _context.Users.Find(userId);
    //     if (user == null)
    //     {
    //         ModelState.AddModelError("User", "No User Found!");
    //         return false;
    //     }

    //     // var userRole = user.Role;
    //     // if(userRole != "Admin" && userRole == "Student")
    //     // {
    //     //      ModelState.AddModelError("Admin", "No Admin Found!");
    //     //     return false;
    //     // }

    //     return user.Role == "Admin";
    // }

    // public IActionResult Index(string searchString)
    // {
    //     if(!IsUserLoggedIn() || !IsAdmin())
    //     {
    //         return RedirectToAction("Login","User");
    //     }

    //     var addresses = _addressService.GetAllAddresses();

    //     if(!string.IsNullOrEmpty(searchString))
    //     {
    //         addresses = addresses.Where( a =>
    //         a.City.ToLower().Contains(searchString.ToLower()) ||
    //         a.State.ToLower().Contains(searchString.ToLower()) ||
    //         a.Town.ToLower().Contains(searchString.ToLower()) ||
    //         a.Id.ToString().Contains(searchString)
            
    //         );
    //     }

    //     var viewModel = new AddressListViewModel
    //     {
    //         Addresses = addresses,
    //         ShowFullList = string.IsNullOrEmpty(searchString)
    //     };

    //     return View(viewModel);
    // }

    // public IActionResult Create()
    // {
    //     if(!IsUserLoggedIn() ||!IsAdmin())
    //     {
    //         return RedirectToAction("Login","User");
    //     }

    //     var addressViewModel = new AddressViewModel();
    //     return View(addressViewModel);
    // }

    // [HttpPost]
    // public async Task<IActionResult> Create(AddressViewModel addressViewModel)
    // {
    //     if(!IsUserLoggedIn() ||!IsAdmin())
    //     {
    //         return RedirectToAction("Login","User");
    //     }
    //     if (ModelState.IsValid)
    //     {
    //         var address = new Address
    //         {
    //             City = addressViewModel.City,
    //             Town = addressViewModel.Town,
    //             State = addressViewModel.State
    //         };

    //         _addressService.CreateAddress(address);
    //         return RedirectToAction("Index");
    //     }

    //     return View(addressViewModel);
    // }

    // public async Task<IActionResult> Edit(int id)
    // {
    //     if(!IsUserLoggedIn()||!IsAdmin())
    //     {
    //         return RedirectToAction("Login","User");
    //     }

    //     var address = await _addressService.GetAddressById(id);
    //     if(address == null)
    //     {
    //         return NotFound();
    //     }

    //     var addressViewModel = new AddressViewModel
    //     {
    //         Id = address.Id,
    //         City = address.City,
    //         Town = address.Town,
    //         State = address.State
    //     };

    //     return View(addressViewModel);

    // }

    // [HttpPost]
    // public async Task<IActionResult> Edit (int id, AddressViewModel addressViewModel)   
    // {
    //     if(!IsUserLoggedIn()||!IsAdmin())
    //     {
    //         return RedirectToAction("Login","User");
    //     }
        
    //     if(ModelState.IsValid)
    //     {
    //         var address = await _addressService.GetAddressById(id);
    //         if(address == null)
    //         {
    //             return NotFound();
    //         }

    //         address.City = addressViewModel.City;
    //         address.Town = addressViewModel.Town;
    //         address.State = addressViewModel.State;

    //         await _addressService.UpdateAddress(address);
    //         return RedirectToAction("Index");
    //     }

    //     return View(addressViewModel);  // reRender with errors
    // }

    // public async Task<IActionResult> Delete (int id)
    // {
    //      if(!IsUserLoggedIn()||!IsAdmin())
    //     {
    //         return RedirectToAction("Login","User");
    //     }
        
    //     var address = await _addressService.GetAddressById(id);
    //     if (address == null)
    //     {
    //         return NotFound();
    //     }

    //     await _addressService.DeleteAddress(id);

    //     return RedirectToAction("Index");
    
    // }
}