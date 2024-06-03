namespace StudentRegisteration.ViewModels
{
    public class CourseOfferingCreateViewModel
    {
        public int? Id  { get; set; }
        public string AcademicYear  { get; set; }
        public string Semester       { get; set; }
        public string CourseName    { get; set; }

       public ICollection<CourseOfferingCreateViewModel>? CombinedData  { get; set; }
    }
}