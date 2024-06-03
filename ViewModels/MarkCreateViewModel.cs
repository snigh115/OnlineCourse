namespace StudentRegisteration.ViewModels
{
    public class MarkCreateViewModel
    {
        public string StudentName { get; set; }
        public string StudentDetailsId { get; set; }
        public string RollNumber { get; set; }
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string AcademicYear { get; set; }
        public string Semester { get; set; }

        public int? MarkValue { get; set; }
        public bool IsPassed { get; set; }

       
    }
}