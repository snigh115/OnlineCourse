using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface IEnrollmentCourseRepository
    {
        Task<List<string>> GetUniqueCourseNamesAsync();
        Task<IEnumerable<EnrollmentCourse>> GetAllEnrollmentCoursesAsync();

        Task<List<EnrollmentCourse>> GetEnrollmentCoursesByCourseNameAsync(string courseName);
        Task<List<EnrollmentCourse>> GetEnrollmentCoursesByAcademicYearAsync(string academicYear);
        Task<List<EnrollmentCourse>> GetEnrollmentCoursesBySemesterAsync(string semester);
        Task<List<EnrollmentCourse>> GetEnrollmentCoursesByStatusAsync(string status);
        Task<List<EnrollmentCourse>> GetEnrollmentCoursesByIsPassedAsync(bool isPassed);


        Task<EnrollmentCourse> AddEnrollmentCourseAsync(EnrollmentCourse enrollmentCourse);
        Task<EnrollmentCourse> UpdateEnrollmentCourseAsync(EnrollmentCourse enrollmentCourse);
        Task DeleteEnrollmentCourseAsync(EnrollmentCourse enrollmentCourse);
        Task<int> GetEnrollmentCountByAcademicYearAndSemesterAsync(string academicYear, string semester);
        Task<int> GetEnrollmentCountByAcademicYearAsync(string academicYear);
        Task<int> GetUniqueEnrollmentCountByAcademicYearAsync(string academicYear);

        Task<int> GetTotalEnrollmentCountsAsync();

        Task<IEnumerable<EnrollmentCourse>> GetEnrollmentCourseByEnrollmentId(int enrollmentId);
        
        Task UpdateEnrollmentStatusAsync(int enrollmentId, string status);   
    }
}