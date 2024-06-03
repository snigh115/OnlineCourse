namespace StudentRegisteration.ViewModels
{
    public class EnrollmentCourseDetailsViewModel
    {
        public string Name  { get; set; }
        public string Email  { get; set; }
        public string NrcNumber   { get; set; }
        public string PhoneNumber  { get; set; }
        public string AcademicYear  { get; set; }
        public string Semester   { get; set; }  
        public List<string> CourseName  { get; set; }
        public List<int?> Mark { get; set; }
        public List<bool?> IsPassed  { get; set; }
        
        public double Cost  { get; set; }
        public double TechnicalFee { get; set; }
        public double TransitionFee { get; set; }
        public double ActualCost { get; set; }
        public string PaymentType  { get; set; }

        public string Status { get; set; } 

        public string RollNumber  { get; set; }

        public string ProfilePicPath { get; set; }

      
    }
}