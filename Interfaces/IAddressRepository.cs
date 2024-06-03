using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressByIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task AddAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(int id);

    }
}