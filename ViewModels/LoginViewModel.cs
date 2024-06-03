namespace  StudentRegisteration.ViewModels;

using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{ 
    [Required(ErrorMessage = "Username or Email is required.")]
    public string UsernameOrEmail { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}