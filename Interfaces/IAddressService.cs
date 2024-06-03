using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces;

public interface IAddressService
{
    
    Task<Address> GetAddressByIdAsync(int id);
    Task<IEnumerable<Address>> GetAllAddressesAsync();  // Optional filtering by state
    Task AddAddressAsync(Address address);
    Task UpdateAddressAsync(Address address);
    Task DeleteAddressAsync(int id);

    Task<IEnumerable<Address>> GetAddressesByFiltersAsync(string state, string city, string town);

    Task<IEnumerable<string>> GetDistinctStatesAsync();

    Task<IEnumerable<string>> GetDistinctCitiesAsync();

    Task<IEnumerable<string>> GetDistinctTownsAsync();

    Task<IEnumerable<string>> GetCitiesByStateAsync(string state);

    Task<IEnumerable<string>> GetTownsByStateAsync(string state);

    Task<IEnumerable<string>> GetStatesByCityAsync(string city);

    Task<IEnumerable<string>> GetTownByCityAsync(string city);

    Task<IEnumerable<string>> GetStatesByTownAsync(string town);

    Task<IEnumerable<string>> GetCitiesByTownAsync(string town);

    Task<int?> FindAddressesIdByFiltersAsync(string state, string city, string town);

    Task<int?> FindAddressIdByFiltersAsync(string state, string city, string town);

    Task<Address> GetAddressDetailsByAddressIdAsync(int addressId);
}

// IEnumerable<Address> GetAllAddresses();
    // IEnumerable<Address> SearchAddresses(string searchString);
    // Task<Address> GetAddressById(int id);
    // void CreateAddress(Address address);
    // Task UpdateAddress(Address address);
    // Task DeleteAddress(int id);