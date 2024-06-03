using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.ViewModels;

namespace StudentRegisteration.Services
{
    public class CourseOfferingService : ICourseOfferingService
    {
        private readonly ICourseOfferingRepository _courseOfferingRepository;
        private readonly ICourseRepository _courseRepository;
        public CourseOfferingService(ICourseOfferingRepository courseOfferingRepository,ICourseRepository courseRepository)
        {
            _courseOfferingRepository = courseOfferingRepository;
            _courseRepository = courseRepository;
        }

        public async Task<CourseOffering> GetCourseOfferingByIdAsync(int id)
        {
            return await _courseOfferingRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CourseOffering>> GetAllCourseOfferingsAsync()
        {
            return await _courseOfferingRepository.GetAllAsync();
        }

        public async Task CreateCourseOfferingAsync(CourseOffering courseOffering)
        {
            await _courseOfferingRepository.CreateAsync(courseOffering);
        }

        public async Task UpdateCourseOfferingAsync(CourseOffering courseOffering)
        {
            await _courseOfferingRepository.UpdateAsync(courseOffering);
        }

        public async Task DeleteCourseOfferingAsync(int id)
        {
            await _courseOfferingRepository.DeleteAsync(id);
        }

        public async Task<int?> GetCourseOfferingIdAsync(string academicYear, string semester, string courseName)
        {
            return await _courseOfferingRepository.GetCourseOfferingIdAsync(academicYear, semester, courseName);
        }

        public async Task<IEnumerable<CourseOfferingCreateViewModel>> GetCourseOfferingsByFiltersAsync(string courseName, string academicYear, string semester)
        {
            var courses = await _courseRepository.GetAllAsync();
            var courseOfferings = await _courseOfferingRepository.GetAllAsync();

            var combinedData = from co in courseOfferings
                                join c in courses on co.Id equals c.CourseOfferingId
                                select new 
                                {
                                    CourseOfferingId = co.Id,
                                    AcademicYear = co.AcademicYear,
                                    Semester = co.Semester,
                                    CourseName = c.Name,

                                };

            if (!string.IsNullOrEmpty(courseName))
            {
                combinedData = combinedData.Where(co => co.CourseName == courseName);
            }

            if (!string.IsNullOrEmpty(academicYear))
            {
                combinedData = combinedData.Where(co => co.AcademicYear == academicYear);
            }

            if (!string.IsNullOrEmpty(semester))
            {
                combinedData = combinedData.Where(co => co.Semester == semester);
            }

            return combinedData.Select(co => new CourseOfferingCreateViewModel
            {
                Id = co.CourseOfferingId,
                AcademicYear = co.AcademicYear,
                Semester = co.Semester,
                CourseName = co.CourseName,
            }).ToList();
            
        }

        public async Task<IEnumerable<string>> GetDistinctAcademicYearsAsync()
        {
            return await _courseOfferingRepository.GetDistinctAcademicYearsAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctSemestersAsync()
        {
            return await _courseOfferingRepository.GetDistinctSemestersAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctCourseNamesAsync()
        {
            return await _courseOfferingRepository.GetDistinctCourseNamesAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctCourseNamesAsyncByAcademicYearAndSemester(string academicYear, string semester)
        {
            return await _courseOfferingRepository.GetDistinctCourseNamesAsyncByAcademicYearAndSemester(academicYear, semester);
        }
    }
}

