namespace StudentRegisteration.Models
{
    public class Mark
    {
        public int Id  { get; set; }
        public int MarkValue  { get; set; }
        public bool IsPassed   { get; set; }
        // public int CourseId  { get; set; }
        // public ICollection<CourseMark> CourseMarks  { get; set; }

        public ICollection<EnrollmentCourse> EnrollmentCourses  { get; set; }
    }
}