using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface ICourseOfferingRepository
    {
        Task<CourseOffering> GetByIdAsync(int id);
        Task<IEnumerable<CourseOffering>> GetAllAsync();
        Task CreateAsync(CourseOffering courseOffering);
        Task UpdateAsync(CourseOffering courseOffering);
        Task DeleteAsync(int id);
        Task<int?> GetCourseOfferingIdAsync(string academicYear, string semester, string courseName);

        Task<IEnumerable<string>> GetDistinctAcademicYearsAsync();
        Task<IEnumerable<string>> GetDistinctSemestersAsync();
        Task<IEnumerable<string>> GetDistinctCourseNamesAsync();
        Task<IEnumerable<string>> GetDistinctCourseNamesAsyncByAcademicYearAndSemester(string academicYear, string semester);
    }
}