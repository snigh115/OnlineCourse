using System.ComponentModel.DataAnnotations;
using StudentRegisteration.Models;

namespace StudentRegisteration.ViewModels
{
    public class StudentDetailsViewModel
    {
    public string Id { get; set; }

    [Required(ErrorMessage ="Name is required.")]
    public string Name { get; set; }

    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Nrc Number")]
    public string NrcNumber  { get; set; }

    [Display(Name = "Nrc Front Pic")]
    public string NrcFrontPicPath { get; set; }

    [Display(Name = "Nrc Back Pic")]
    public string NrcBackPicPath { get; set; }

    [Display(Name = "Profile Pic")]
    public string ProfilePicPath  { get; set; }

    //Generate Number
    public string RegistrationNumber  { get; set; }// Datetime.now ,AcademicYear 

    public DateTime RegistrationDate  { get; set; } // = DateTime.Now

    public string UserId { get; set; } //FK
    public User User { get; set; }
    
    [Required(ErrorMessage = "State is required")]
    public string? State { get; set; }

    [Required(ErrorMessage = "City is required")]
    public string? City { get; set; }

    [Required(ErrorMessage = "Town is required")]
    public string? Town { get; set; }

    public List<Enrollment>? Enrollments  { get; set; }
    }
}