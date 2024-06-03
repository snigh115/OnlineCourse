using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public  AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            return await _context.Addresses
                .Include(a => a.StudentDetails)  // eager loading for the one-to-one
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            return await _context.Addresses
                .Include(a => a.StudentDetails)    // include related data - required to show details
                .ToListAsync();
        }

        public async Task AddAddressAsync(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(int id)
        {
            var address = await GetAddressByIdAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
        
    }
}