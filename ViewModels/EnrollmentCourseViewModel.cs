namespace StudentRegisteration.ViewModels
{
    public class EnrollmentCourseViewModel
    {
        public string StudentName { get; set; }
        public string StudentDetailsId { get; set; }
        public string RollNumber { get; set; }
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string AcademicYear { get; set; }
        public string Semester { get; set; }
        public bool IsCompleted { get; set; }
        public string Status { get; set; }

        public int? MarkValue { get; set; }
        public bool? IsPassed { get; set; }

        
    public int Page { get; set; }
    public int TotalCount { get; set; }

//public List<Marks>
        public List<string> AvailableStatuses { get; set; } = new List<string>() 
            {
                "Pending", "Approved", "Rejected"
            };
    }
}