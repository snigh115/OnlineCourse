using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Repository
{
    public class EnrollmentCourseRepository : IEnrollmentCourseRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentCourseRepository(ApplicationDbContext context) => _context = context;  

        public async Task<List<string>> GetUniqueCourseNamesAsync()
        {
            return await _context.EnrollmentCourses
                .Select(ec => ec.Course.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<EnrollmentCourse>> GetAllEnrollmentCoursesAsync()
        {
            return await _context.EnrollmentCourses
                .Include(ec => ec.Enrollment)
                    .ThenInclude(e => e.StudentDetails)
                .Include(ec => ec.Course)
                    .ThenInclude(c => c.CourseOfferings)
                .Include(ec => ec.Mark)
                    
                .ToListAsync();
        }

        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesByCourseNameAsync(string courseName)
        {
            return await _context.EnrollmentCourses
                .Where(ec => ec.Course.Name == courseName)
                .Include(ec => ec.Enrollment)
                    .ThenInclude(e => e.StudentDetails)
                .Include(ec => ec.Course)
                    .ThenInclude(c => c.CourseOfferings)
                .Include(ec => ec.Mark)
                .ToListAsync();
        }

        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesByAcademicYearAsync(string academicYear)
        {
            return await _context.EnrollmentCourses
                .Where(ec => ec.Course.CourseOfferings.AcademicYear == academicYear)
                .Include(ec => ec.Enrollment)
                    .ThenInclude(e => e.StudentDetails)
                .Include(ec => ec.Course)
                    .ThenInclude(c => c.CourseOfferings)
                .Include(ec => ec.Mark)
                .ToListAsync();
        }

        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesBySemesterAsync(string semester)
        {
            return await _context.EnrollmentCourses
                .Where(ec => ec.Course.CourseOfferings.Semester == semester)
                .Include(ec => ec.Enrollment)
                    .ThenInclude(e => e.StudentDetails)
                .Include(ec => ec.Course)
                    .ThenInclude(c => c.CourseOfferings)
                .Include(ec => ec.Mark)
                .ToListAsync();
        }

        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesByStatusAsync(string status)
        {
            return await _context.EnrollmentCourses
                .Where(ec => ec.Enrollment.Status == status)
                .Include(ec => ec.Enrollment)
                    .ThenInclude(e => e.StudentDetails)
                .Include(ec => ec.Course)
                    .ThenInclude(c => c.CourseOfferings)
                .Include(ec => ec.Mark)
                .ToListAsync();
        }

        public async Task<List<EnrollmentCourse>> GetEnrollmentCoursesByIsPassedAsync(bool isPassed)
        {
            return await _context.EnrollmentCourses
                .Where(ec => ec.Mark.IsPassed == isPassed)
                .Include(ec => ec.Enrollment)
                    .ThenInclude(e => e.StudentDetails)
                .Include(ec => ec.Course)
                    .ThenInclude(c => c.CourseOfferings)
                .Include(ec => ec.Mark)
                .ToListAsync();
        }

        public async Task<EnrollmentCourse> AddEnrollmentCourseAsync(EnrollmentCourse enrollmentCourse) 
        {
            _context.EnrollmentCourses.Add(enrollmentCourse);
            await _context.SaveChangesAsync();
            return enrollmentCourse;
        }

        public async Task<EnrollmentCourse> UpdateEnrollmentCourseAsync(EnrollmentCourse enrollmentCourse)  
        {
            _context.EnrollmentCourses.Update(enrollmentCourse);
            await _context.SaveChangesAsync();
            return enrollmentCourse;
        }

        public async Task DeleteEnrollmentCourseAsync(EnrollmentCourse enrollmentCourse)
        {
            _context.EnrollmentCourses.Remove(enrollmentCourse);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetEnrollmentCountByAcademicYearAndSemesterAsync(string academicYear, string semester)
        {
            return await _context.EnrollmentCourses
                .Where(ec => ec.Course.CourseOfferings.AcademicYear == academicYear && ec.Course.CourseOfferings.Semester == semester)  
                .CountAsync();
        }

        public async Task<int> GetEnrollmentCountByAcademicYearAsync(string academicYear)
        {
            return await _context.EnrollmentCourses
                .Where(ec => ec.Course.CourseOfferings.AcademicYear == academicYear)
                .CountAsync();
        }

        public async Task<int> GetTotalEnrollmentCountsAsync()
        {
            return await _context.EnrollmentCourses
                .GroupBy(ec => ec.EnrollmentId)
                .CountAsync();
        }

        public async Task<int> GetUniqueEnrollmentCountByAcademicYearAsync(string academicYear)
        {
            return await _context.EnrollmentCourses
                .Where(ec => ec.Course.CourseOfferings.AcademicYear == academicYear )
                .GroupBy(ec => ec.EnrollmentId)
                .Select(group => group.Key)
                .CountAsync();
        }


        public async Task<IEnumerable<EnrollmentCourse>> GetEnrollmentCourseByEnrollmentId(int enrollmentId)
        {
            var enrollments = await _context.EnrollmentCourses
                .Where(ec => ec.EnrollmentId == enrollmentId)
                .Include(ec => ec.Enrollment)
                    .ThenInclude(e => e.StudentDetails)
                    .ThenInclude(sd => sd.User)
                .Include(ec => ec.Course)
                
                    .ThenInclude(c => c.CourseOfferings)
                .Include(ec => ec.Mark)
                
                
              
                    
                .ToListAsync();

            return enrollments;
        }

        public async Task UpdateEnrollmentStatusAsync(int enrollmentId, string status)
        {
            var enrollment = await _context.EnrollmentCourses
                .Include(ec => ec.Enrollment)
                .FirstOrDefaultAsync(ec => ec.EnrollmentId == enrollmentId);

            if (enrollment != null)
            {
                enrollment.Enrollment.Status = status;
                await _context.SaveChangesAsync();
            }
        }

       
       



    }
}