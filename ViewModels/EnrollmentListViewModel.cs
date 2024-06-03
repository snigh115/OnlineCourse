namespace StudentRegisteration.ViewModels
{
    public class EnrollmentListViewModel
    {
        public List<EnrollmentCourseViewModel> enrollmentCourseViewModels { get; set; }
        public EnrollmentFilterOptionsViewModel FilterOptions { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }

        public EnrollmentListViewModel()
        {
            FilterOptions = new EnrollmentFilterOptionsViewModel();
        }
    }
}