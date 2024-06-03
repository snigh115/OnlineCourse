using System.ComponentModel.DataAnnotations;

namespace StudentRegisteration.Models;

public class User
{
    public string Id { get; set; }

    [Required(ErrorMessage = "UserName is required.")]
    public string UserName { get; set; }

    [Required(ErrorMessage ="Email is required.")]
    [EmailAddress(ErrorMessage ="Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public string Role { get; set; }  // Admin , Student

    //for make Admin not to be hard coded
    public static readonly string StudentRole = "Student";
    public static readonly string AdminRole = "Admin";

    // Optional for one to one 
    public StudentDetails StudentDetails { get; set; }

}