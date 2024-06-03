using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Services;


public class AddressServices : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressServices(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Address> GetAddressByIdAsync(int id)
    {
        return await _addressRepository.GetAddressByIdAsync(id);
    }

    public async Task<IEnumerable<Address>> GetAllAddressesAsync()
    {
        var addresses = await _addressRepository.GetAllAddressesAsync();
      
        return addresses;
    }

    public async Task AddAddressAsync(Address address)
    {
        await _addressRepository.AddAddressAsync(address);
    }

    public async Task UpdateAddressAsync(Address address)
    {
        await _addressRepository.UpdateAddressAsync(address);
    }

    public async Task DeleteAddressAsync(int id)
    {
        await _addressRepository.DeleteAddressAsync(id);
    }

    public async Task<IEnumerable<Address>> GetAddressesByFiltersAsync(string state, string city, string town)
    {
        var addresses = await _addressRepository.GetAllAddressesAsync();

        if (!string.IsNullOrEmpty(state))
        {
            addresses = addresses.Where( a => a.State == state);
        }

        if (!string.IsNullOrEmpty(city))
        {
            addresses = addresses.Where( a => a.City == city);
        }

        if (!string.IsNullOrEmpty(town))
        {
            addresses = addresses.Where( a => a.Town == town);
        }

        return addresses;
    }

    public async Task<IEnumerable<string>> GetDistinctStatesAsync()
    {
        var addresses = await _addressRepository.GetAllAddressesAsync();
        return addresses.Select(a => a.State).Distinct();
    }

    public async Task<IEnumerable<string>> GetDistinctCitiesAsync()
    {
        var addresses = await _addressRepository.GetAllAddressesAsync();
        return addresses.Select(a => a.City).Distinct();
    }

    public async Task<IEnumerable<string>> GetDistinctTownsAsync()
    {
        var addresses = await _addressRepository.GetAllAddressesAsync();
        return addresses.Select(a => a.Town).Distinct();
    }

    public async Task<IEnumerable<string>> GetCitiesByStateAsync(string state)
    {
        if (string.IsNullOrEmpty(state))
        {
            return Enumerable.Empty<string>();
        }

        var addresses = await  _addressRepository.GetAllAddressesAsync();
        return addresses.Where( a => a.State == state).Select(a => a.City).Distinct();
    }

    public async Task<IEnumerable<string>> GetTownsByStateAsync(string state)
    {
        if (string.IsNullOrEmpty(state))
        {
            return Enumerable.Empty<string>();
        }
        
        var addresses = await _addressRepository.GetAllAddressesAsync();
        return addresses.Where(a => a.State == state).Select(a => a.Town).Distinct();
    }

    public async Task<IEnumerable<string>> GetStatesByCityAsync(string city)
    {
        if (string.IsNullOrEmpty(city))
        {
            return Enumerable.Empty<string>();
        }

        var addresses = await _addressRepository.GetAllAddressesAsync();
        return addresses.Where(a => a.City == city).Select(a => a.State).Distinct();
    }

    public async Task<IEnumerable<string>> GetTownByCityAsync(string city)
    {
        if (string.IsNullOrEmpty(city))
        {
            return Enumerable.Empty<string>();
        }

        var addresses = await _addressRepository.GetAllAddressesAsync();
        return addresses.Where( a => a.City == city).Select(a => a.Town).Distinct();
    }

    public async Task<IEnumerable<string>> GetCitiesByTownAsync(string town)
    {
        if (string.IsNullOrEmpty(town))
        {
            return Enumerable.Empty<string>();
        }

        var addresses = await _addressRepository.GetAllAddressesAsync();
        return addresses.Where(a => a.Town == town).Select(a => a.City).Distinct();
    }

    public async Task<IEnumerable<string>> GetStatesByTownAsync(string town)
    {
        if (string.IsNullOrEmpty(town))
        {
            return Enumerable.Empty<string>();
        }

        var addresses = await _addressRepository.GetAllAddressesAsync();
        return addresses.Where(a => a.Town == town).Select(a => a.State).Distinct();
    }

    public async Task<int?> FindAddressesIdByFiltersAsync(string state, string city, string town)
    {
        var addresses = await GetAddressesByFiltersAsync(state, city , town);

        foreach (var address in addresses)
        {
            if (address.State == state && address.City == city && (town == null || address.Town == town))
            {
                return address.Id;
            }
        }

        return null;
    }

    public async Task<int?> FindAddressIdByFiltersAsync(string state, string city, string town)
    {
        var addresses = await GetAddressesByFiltersAsync(state, city, town);

        var matchingAddress = addresses.FirstOrDefault( a => 
                                            a.State == state &&
                                            a.City == city && 
                                            (town == null || a.Town == town)
                                        );

        return matchingAddress?.Id;
    }

    public async Task<Address> GetAddressDetailsByAddressIdAsync(int addressId)
    {
        var addressDetails = await _addressRepository.GetAddressByIdAsync(addressId);

        return addressDetails;
    }
}


    // public IEnumerable<Address> GetAllAddresses()
    // {
    //     return _context.Addresses.ToList();
    // }

    // public IEnumerable<Address> SearchAddresses(string searchString)
    // {
    //     if(string.IsNullOrEmpty(searchString))
    //     {
    //         return GetAllAddresses();
    //     }

    //     return _context.Addresses.Where( a => 
    //         a.City.ToLower().Contains(searchString.ToLower()) ||
    //         a.State.ToLower().Contains(searchString.ToLower()) ||
    //         a.Town.ToLower().Contains(searchString.ToLower()) ||
    //         a.Id.ToString().Contains(searchString)
    //     );
    // }

    // public async Task<Address> GetAddressById(int id)
    // {
    //     return await _context.Addresses.FindAsync(id);
    // }

    // public void CreateAddress(Address address)
    // {
    //     _context.Addresses.Add(address);
    //     _context.SaveChanges();
    // }

    // public async Task UpdateAddress(Address address)
    // {
    //     _context.Addresses.Update(address);
    //     _context.SaveChanges();
    // }

    // public async Task DeleteAddress(int id)
    // {
    //     var address = _context.Addresses.Find(id);
    //     if(address != null)
    //     {
    //         _context.Addresses.Remove(address);
    //         _context.SaveChanges();
    //     }
    // }

