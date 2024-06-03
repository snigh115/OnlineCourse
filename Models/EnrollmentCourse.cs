namespace StudentRegisteration.Models
{
    public class EnrollmentCourse
    {
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int? MarkId { get; set; }
        public Mark Mark { get; set; }
    }
}