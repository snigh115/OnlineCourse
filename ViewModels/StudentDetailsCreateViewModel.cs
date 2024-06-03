using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StudentRegisteration.Models;
using StudentRegisteration.Validators;

namespace StudentRegisteration.ViewModels
{
    public class StudentDetailsCreateViewModel
    {
        public string? Id  { get; set; }

        [Required(ErrorMessage ="Name is required.")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

//Check Validation for NrcNumber
        [Required]
        [Display(Name = "Nrc Number")]
        [CustomValidation(typeof(NrcValidation),"ValidateNrcNumber", ErrorMessage ="Invalid NRC number format.")]
        public string NrcNumber  { get; set; }

// Upload Pic
        [Display(Name = "Nrc Front Pic")]
        [Required(AllowEmptyStrings = true)]
        [BindNever]
        public string? NrcFrontPicPath { get; set; }

        [Display(Name = "Nrc Back Pic")]
        [Required(AllowEmptyStrings = true)]
        [BindNever]
        public string? NrcBackPicPath { get; set; }

        [Display(Name = "Profile Pic")]
        [Required(AllowEmptyStrings = true)]
        [BindNever]
        public string? ProfilePicPath  { get; set; }

//Generate Number
        public string? RegistrationNumber  { get; set; }// Datetime.now ,AcademicYear 

        public DateTime RegistrationDate  { get; set; } // = DateTime.Now

        
        public string? UserId  { get; set; }
        // public User User  { get; set; }

        public int? AddressId   { get; set; }
        // public Address Address   { get; set; }

// Get Address instead of Address Address 

        [Required(ErrorMessage = "State is required")]
        public string? State { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Town is required")]
        public string? Town { get; set; }

        public List<Enrollment>? Enrollments  { get; set; }

        public IEnumerable<string> States  { get; set;}

        public IEnumerable<string>? Cities  { get; set;}

        public IEnumerable<string>? Towns  { get; set;}

        public StudentDetailsCreateViewModel()
        {
                States = new List<string>();
                Cities = new List<string>();
                Towns = new List<string>();
        }

    }
}