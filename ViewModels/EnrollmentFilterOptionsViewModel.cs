namespace StudentRegisteration.ViewModels;

public class EnrollmentFilterOptionsViewModel
{
    public string CourseName { get; set; }
    public List<string> CourseNames { get; set; }
    public string AcademicYear { get; set; }
    public string Semester { get; set; }
    public string Status { get; set; }
    public bool? IsPassed { get; set; }
    public string StudentName { get; set; }
    public string RollNumber { get; set; }
}