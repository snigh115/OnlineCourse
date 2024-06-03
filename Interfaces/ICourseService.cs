using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces;

public interface ICourseService
{
    Task<Course> GetCourseByIdAsync(int id);
    Task<IEnumerable<Course>> GetAllCourseAsync();
    Task CreateCourseAsync(Course course);
    Task UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(int id);

    Task<Course> GetCourseByCourseOfferingIdAsync(int courseOfferingId);

    Task<int?> GetCourseIdByFilteringAsync(string academicYear, string semester, string courseName);
}

    // IEnumerable<CourseOffering> GetAllCourses();
    // IEnumerable<CourseOffering> SearchCourses(string searchString);
    // Task<CourseOffering> GetCourseById(int id);
    // void CreateCourse(CourseOffering course);
    // Task UpdateCourse(CourseOffering course);
    // Task DeleteCourse(int id);