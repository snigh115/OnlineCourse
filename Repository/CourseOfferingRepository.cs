using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Repository
{
    public class CourseOfferingRepository : ICourseOfferingRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseOfferingRepository(ApplicationDbContext context) => _context = context;

        public async Task<CourseOffering> GetByIdAsync(int id)
        {
            return await _context.Set<CourseOffering>()
                .Include(co => co.Courses)       
                .FirstOrDefaultAsync(co => co.Id == id);
        }

        public async Task<IEnumerable<CourseOffering>> GetAllAsync()
        {
            return await _context.Set<CourseOffering>()
                .Include(co => co.Courses)       
                .ToListAsync();
        }

        public async Task CreateAsync(CourseOffering courseOffering)
        {
            await _context.Set<CourseOffering>().AddAsync(courseOffering);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseOffering courseOffering)
        {
            _context.Set<CourseOffering>().Update(courseOffering);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var courseOffering = await GetByIdAsync(id);
            if (courseOffering != null)
            {
                _context.Set<CourseOffering>().Remove(courseOffering);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int?> GetCourseOfferingIdAsync(string academicYear, string semester, string courseName)
        {
            var courseOfferingId = await _context.Set<CourseOffering>()
                .Where(co => co.AcademicYear == academicYear && co.Semester == semester)
                .SelectMany(co => co.Courses)
                .Where(c => c.Name == courseName)
                .Select(c => c.CourseOfferingId)
                .FirstOrDefaultAsync();

            return courseOfferingId;
        }

        public async Task<IEnumerable<string>> GetDistinctAcademicYearsAsync()
        {
            return await _context.CourseOfferings
                .Select(co => co.AcademicYear)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctSemestersAsync()
        {
            return await _context.CourseOfferings
                .Select(co => co.Semester)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctCourseNamesAsync()
        {
            return await _context.CourseOfferings
                .SelectMany(co => co.Courses)
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctCourseNamesAsyncByAcademicYearAndSemester(string academicYear, string semester)
        {
            return await _context.CourseOfferings
                .Where(co => co.AcademicYear == academicYear && co.Semester == semester)
                .SelectMany(co => co.Courses)
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }
    }
}