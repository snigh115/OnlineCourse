using System.ComponentModel.DataAnnotations;

namespace StudentRegisteration.Models;

public class Enrollment
{
    public int Id { get; set; }
    public bool Completed  { get; set; } = false;
    public string RollNumber  { get; set; }

    public string Status { get; set; } //= "Pending"; // Pending, Approved, Rejected

    public DateTime EnrollmentDate  { get; set; } // = DateTime.Now

    public DateTime? ApprovalDate  { get; set;}//Approve Date

    public ICollection<EnrollmentCourse> EnrollmentCourses { get; set; }

    public string StudentDetailsId { get; set; } //FK
    public StudentDetails StudentDetails  { get; set; }

    public double Cost  { get; set; }
    public string PaymentType   { get; set; }

    // public int? MarkValue { get; set; }
    // public bool? IsPassed { get; set; }

    public static readonly string PendingStatus = "Pending";
    public static readonly string ApprovedStatus = "Approved";
    public static readonly string RejectedStatus = "Rejected";

}