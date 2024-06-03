using StudentRegisteration.Models;
using StudentRegisteration.Models.Class;

namespace StudentRegisteration.Interfaces;

public interface IUserService<TUser> where TUser : class
{
    Task<RegistrationResult> Register(TUser user);
    Task<LoginResult> Login(string userNameOrEmail,string password);
    Task<TUser> GetUserById(string userId);
    Task<TUser> GetUserByEmail(string email);

}