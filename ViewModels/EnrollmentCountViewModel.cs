namespace StudentRegisteration.ViewModels
{
    public class EnrollmentCountViewModel
    {
        public string AcademicYear { get; set; }
        public string Semester { get; set; } // Optional
        public int EnrollmentCount { get; set; }
        public int UniqueEnrollmentCount  { get; set; } 

        public bool Completed { get; set; }
    }

}