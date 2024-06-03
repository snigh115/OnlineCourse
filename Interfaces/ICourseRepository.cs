using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task CreateAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);

        Task<Course> GetCourseByCourseOfferingIdAsync(int courseOfferingId);

        Task<int?> GetCourseIdByFilteringAsync(string academicYear, string semester, string courseName);    
    }
}