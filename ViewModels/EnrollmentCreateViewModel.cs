using System.ComponentModel.DataAnnotations;
using StudentRegisteration.Models;

namespace StudentRegisteration.ViewModels
{
    public class EnrollmentCreateViewModel
    {
        // public int? Id { get; set; }
        public bool Completed  { get; set; } 

        [Required]
        [Display(Name = "Roll Number")]
        public string RollNumber { get; set; }

        public string Status { get; set; } = Enrollment.PendingStatus;//= "Pending"; // Pending, Approved, Rejected

        public DateTime EnrollmentDate  { get; set; }  // = DateTime.Now

        public DateTime? ApprovalDate  { get; set;}//Approve Date

        public string AcademicYear  { get; set; }
        public string Semester   { get; set; }
        public IEnumerable<string> CourseName  { get; set; } = Enumerable.Empty<string>();

        public int MarkValue { get; set; } 
        public bool IsPassed { get; set; }


        public IEnumerable<string>? AcademicYears  {get;set;}
        public IEnumerable<string>? Semesters { get; set; }
        public IEnumerable<string>? CourseNames { get; set; }
       
        [Required]
        [Display(Name = "Student Details")]
        public string StudentDetailsId { get; set; } //FK

        public double Cost  { get; set; }

        [Required]
        [Display(Name = "Payment Type")]
        public string PaymentType   { get; set; }

        public List<string> SelectedCourses  { get; set; }
        public Dictionary<string, int> MarkValues  { get; set; }
    }
}