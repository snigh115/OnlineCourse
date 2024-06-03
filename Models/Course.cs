namespace StudentRegisteration.Models
{
    public class Course
    {
        public int Id  { get; set; }
        public string Name  { get; set; } // English, Myanmar, etc
       
        public int CourseOfferingId  { get; set; }  //Fk
        public CourseOffering CourseOfferings  { get; set; }
        
        // public ICollection<CourseMark> CourseMarks   { get; set; }

        public ICollection<EnrollmentCourse> EnrollmentCourses { get; set; }
    }
}