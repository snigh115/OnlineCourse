using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.Models.Class;

namespace StudentRegisteration.Services;

public class UserService : IUserService<User>
{
    private readonly IUserRepository _userRepository;
    

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegistrationResult> Register (User user)
    {
        user.Id = Guid.NewGuid().ToString();
        user.Password = HashPassword(user.Password);
        user.Role = user.Role; // ?? User.StudentRole;  Default to student if no role is provided 

        var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existingUser != null)
        {
            return new RegistrationResult
            {
                IsSuccessful = false,
                ErrorMessage = "Email address  already in use"
            };
        }

        
        await  _userRepository.AddUserAsync(user);

        return new RegistrationResult
        {
            IsSuccessful= true,
            ErrorMessage= null
        };

        // return user.Id;
    }

    public async Task<LoginResult> Login(string userNameOrEmail, string password)
    {
        var user = userNameOrEmail.Contains("@")
            ? await _userRepository.GetUserByEmailAsync(userNameOrEmail)
            : await _userRepository.GetUserByUserNameAsync(userNameOrEmail);

        if(user != null && VerifyPassword(user.Password, password))
        {
            return new LoginResult
            {
                IsAuthenticated = true,
                User = user,
                ErrorMessage = null
            };
        }
        else
        {
            return new LoginResult
            {
                IsAuthenticated = false,
                User = null,
                ErrorMessage = "Invalid  username or password."
            };
        }

    }

    public async Task<User> GetUserById(string userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }

    
    // for Password Hashing
      private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // for password verification
    private bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}