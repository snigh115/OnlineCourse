using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context ) => _context = context;

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Set<Course>().FindAsync(id);
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Set<Course>().ToListAsync(); 
        }

        public async Task CreateAsync(Course course)
        {
            await _context.Set<Course>().AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Set<Course>().Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var course = await GetByIdAsync(id);
            if (course != null)
            {
                _context.Set<Course>().Remove(course);
                await  _context.SaveChangesAsync();
            }
        }

        public async Task<Course> GetCourseByCourseOfferingIdAsync(int courseOfferingId)
        {
            return await _context.Courses
                            .Where(c => c.CourseOfferingId == courseOfferingId)
                            .FirstOrDefaultAsync();
        }

        public async Task<int?> GetCourseIdByFilteringAsync(string academicYear, string semester, string courseName)
        {
            var courseId = await _context.Courses
                .Where( c => c.CourseOfferings.AcademicYear == academicYear &&
                            c.CourseOfferings.Semester == semester &&
                            courseName.Contains(c.Name))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return courseId;
        }
        
    }
}