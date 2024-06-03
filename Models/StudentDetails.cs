using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StudentRegisteration.Validators;

namespace StudentRegisteration.Models;

public class StudentDetails 
{
    public string Id { get; set; }

    [Required(ErrorMessage ="Name is required.")]
    public string Name { get; set; }

    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Nrc Number")]
    [CustomValidation(typeof(NrcValidation),"ValidateNrcNumber", ErrorMessage ="Invalid NRC number format.")]
    public string NrcNumber  { get; set; }

    [Display(Name = "Nrc Front Pic")]
    // [Required(AllowEmptyStrings = true)]
    [BindNever]
    public string NrcFrontPicPath { get; set; }

    [Display(Name = "Nrc Back Pic")]
    // [Required(AllowEmptyStrings = true)]
    [BindNever]
    public string NrcBackPicPath { get; set; }

    [Display(Name = "Profile Pic")]
    // [Required(AllowEmptyStrings = true)]
    [BindNever]
    public string ProfilePicPath  { get; set; }

    //Generate Number
    public string RegistrationNumber  { get; set; }// Datetime.now ,AcademicYear 

    public DateTime RegistrationDate  { get; set; } // = DateTime.Now

    public string UserId { get; set; } //FK
    public User User { get; set; }
    
    public int? AddressId { get; set; } //FK
    public Address Address { get; set; }

    public List<Enrollment>? Enrollments  { get; set; }

} 

