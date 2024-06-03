using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUserNameAsync(string username);
        Task<User> GetUserByIdAsync(string id);
        Task AddUserAsync(User user);
        
    }
}