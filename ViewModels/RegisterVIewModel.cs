namespace  StudentRegisteration.ViewModels;


using System.ComponentModel.DataAnnotations;
using StudentRegisteration.Models;

public class RegisterViewModel
{
    
     [Required(ErrorMessage = "UserName is required.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Confirm password does not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    public string SelectedRole  {get;set;} //= "Student"; //

    public static readonly List<string> Roles = new List<string>() 
                    {
                        User.StudentRole,
                        User.AdminRole
                    };
}