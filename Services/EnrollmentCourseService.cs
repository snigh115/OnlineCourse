using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Services
{
    public class EnrollmentCourseService : IEnrollmentCourseService
    {
        private readonly IEnrollmentCourseRepository _enrollmentCourseRepository;

       

        public EnrollmentCourseService(IEnrollmentCourseRepository enrollmentCourseRepository) => _enrollmentCourseRepository = enrollmentCourseRepository; 

        public async Task<List<string>> GetUniqueCourseNamesAsync()
        {
            return await _enrollmentCourseRepository.GetUniqueCourseNamesAsync();
        }

        public async Task<IEnumerable<EnrollmentCourse>> GetAllEnrollmentCoursesAsync()
        {
            return await _enrollmentCourseRepository.GetAllEnrollmentCoursesAsync();
        }

        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesByCourseNameAsync(string courseName)
        {
            return await _enrollmentCourseRepository.GetEnrollmentCoursesByCourseNameAsync(courseName);
        }
        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesByAcademicYearAsync(string academicYear)
        {
            return await _enrollmentCourseRepository.GetEnrollmentCoursesByAcademicYearAsync(academicYear);
        }
        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesBySemesterAsync(string semester)
        {
            return await _enrollmentCourseRepository.GetEnrollmentCoursesBySemesterAsync(semester);
        }
        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesByStatusAsync(string status)
        {
            return await _enrollmentCourseRepository.GetEnrollmentCoursesByStatusAsync(status);
        }
        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesByIsPassedAsync(bool isPassed)
        {
            return await _enrollmentCourseRepository.GetEnrollmentCoursesByIsPassedAsync(isPassed);
        }
        public async Task<EnrollmentCourse> AddEnrollmentCourseAsync(EnrollmentCourse enrollmentCourse)
        {
            return await _enrollmentCourseRepository.AddEnrollmentCourseAsync(enrollmentCourse);    
        }
        public async Task<EnrollmentCourse> UpdateEnrollmentCourseAsync(EnrollmentCourse enrollmentCourse)
        {
            return await _enrollmentCourseRepository.UpdateEnrollmentCourseAsync(enrollmentCourse); 
        }
        public async Task DeleteEnrollmentCourseAsync(EnrollmentCourse enrollmentCourse)
        {
            await _enrollmentCourseRepository.DeleteEnrollmentCourseAsync(enrollmentCourse); 
        }

        public async Task<int> GetEnrollmentCountByAcademicYearAndSemesterAsync(string academicYear, string semester)
        {
            return await _enrollmentCourseRepository.GetEnrollmentCountByAcademicYearAndSemesterAsync(academicYear, semester); 
        }

        public async Task<int> GetTotalEnrollmentCountsAsync()
        {
            return await _enrollmentCourseRepository.GetTotalEnrollmentCountsAsync();
        }

        public async Task<int> GetEnrollmentCountByAcademicYearAsync(string academicYear)
        {
            return await _enrollmentCourseRepository.GetEnrollmentCountByAcademicYearAsync(academicYear);
        }

        public async Task<int> GetUniqueEnrollmentCountByAcademicYearAsync(string academicYear)
        {
            return await _enrollmentCourseRepository.GetUniqueEnrollmentCountByAcademicYearAsync(academicYear);
        }

        public async Task<IEnumerable<EnrollmentCourse>> GetEnrollmentCourseByEnrollmentId(int enrollmentId)
        {
            return await _enrollmentCourseRepository.GetEnrollmentCourseByEnrollmentId(enrollmentId);
        }

        public async Task UpdateEnrollmentStatusAsync(int enrollmentId, string status)
        {
           await  _enrollmentCourseRepository.UpdateEnrollmentStatusAsync(enrollmentId, status);
        }
    }
}