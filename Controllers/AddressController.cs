using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.ViewModels;

namespace StudentRegisteration.Controllers
{
    public class AddressController : Controller 
    {
        private readonly  IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string state = "", string city = "", string town = "",int page = 1)
        {
           
           
            var states = await _addressService.GetDistinctStatesAsync();
            var cities = await _addressService.GetCitiesByStateAsync(state);
            var towns = await _addressService.GetTownByCityAsync(city);

            IEnumerable<Address> addresses;

            if (string.IsNullOrEmpty(state) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(town))
            {
                addresses = await _addressService.GetAllAddressesAsync();
            }
            else
            {
                addresses = await _addressService.GetAddressesByFiltersAsync(state, city, town);
            }

            int totalCount = addresses.Count();
            int pageSize = 15;

            var paginatedAddresses = addresses.Skip((page -1) * pageSize).Take(pageSize);
            
            var viewModel = new AddressViewModel
            {
                Addresses = paginatedAddresses,  
                States = states,
                Cities = cities,
                Towns = towns,
                State = state,
                City = city,
                Town = town,
                Page = page,
                TotalCount = totalCount,       
            };

            return View (viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index (AddressViewModel model)
        {
            return await Index(model.State, model.City, model.Town);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            if(ModelState.IsValid)
            {
                await _addressService.AddAddressAsync(address);
                return RedirectToAction("Index","Address");
            }

            return View(address);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var address = await  _addressService.GetAddressByIdAsync(id);
            if (address == null)
            {
                return NotFound("Cannot find Address with that id!");
            }

            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Address address)
        {
            if (id  != address.Id)
            {
                return NotFound("Cannot find AddressId!");
            }

            if (ModelState.IsValid)
            {
                try 
                {
                    await _addressService.UpdateAddressAsync(address);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
                    {
                        return NotFound("Address not found.");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Address");
            }
            return View(address);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _addressService.GetAddressByIdAsync(id);
            if (address == null)
            {
                return NotFound("Address does not exist.");
            }

           return View(address);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _addressService.DeleteAddressAsync(id);
            return RedirectToAction("Index","Address");
        }

        private bool AddressExists(int id)
        {
            return _addressService.GetAllAddressesAsync().Result.Any(a => a.Id == id);
        }

    }
}