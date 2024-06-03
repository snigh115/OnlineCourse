namespace StudentRegisteration.Models;

public class CourseOffering
{
    public int Id { get; set;}
  
    public string AcademicYear { get; set; } 
    public string Semester { get; set; }

    public ICollection<Course> Courses  {get;set;} 

}


