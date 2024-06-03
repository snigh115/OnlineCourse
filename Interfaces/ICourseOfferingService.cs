using StudentRegisteration.Models;
using StudentRegisteration.ViewModels;

namespace StudentRegisteration.Interfaces
{
    public interface ICourseOfferingService
    {
        Task<CourseOffering> GetCourseOfferingByIdAsync(int id);
        Task<IEnumerable<CourseOffering>> GetAllCourseOfferingsAsync();
        Task CreateCourseOfferingAsync(CourseOffering courseOffering);
        Task UpdateCourseOfferingAsync(CourseOffering courseOffering);
        Task DeleteCourseOfferingAsync(int id);
        Task<int?> GetCourseOfferingIdAsync(string academicYear, string semester, string courseName);

        Task<IEnumerable<CourseOfferingCreateViewModel>> GetCourseOfferingsByFiltersAsync(string courseName, string academicYear, string semester);

        Task<IEnumerable<string>> GetDistinctAcademicYearsAsync();
        Task<IEnumerable<string>> GetDistinctSemestersAsync();
        Task<IEnumerable<string>> GetDistinctCourseNamesAsync();
        Task<IEnumerable<string>> GetDistinctCourseNamesAsyncByAcademicYearAndSemester(string academicYear, string semester);
        
    }
}