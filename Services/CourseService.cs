using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public  CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Course>> GetAllCourseAsync()
        {
            return await _courseRepository.GetAllAsync();
        }

        public async Task CreateCourseAsync(Course course)
        {
            await _courseRepository.CreateAsync(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _courseRepository.UpdateAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _courseRepository.DeleteAsync(id);
        }

        public async Task<Course> GetCourseByCourseOfferingIdAsync(int courseOfferingId)
        {
            return await _courseRepository.GetCourseByCourseOfferingIdAsync(courseOfferingId);
        }

        public async Task<int?> GetCourseIdByFilteringAsync(string academicYear, string semester, string courseName)
        {
            return await _courseRepository.GetCourseIdByFilteringAsync(academicYear, semester, courseName);
        }
    }
}