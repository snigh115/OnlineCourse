using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context) => _context = context;

        public async Task<Enrollment> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                                // .Include(e => e.CourseOffering)
                                .Include(e => e.StudentDetails)
                                .FirstOrDefaultAsync(e  => e.Id == id);
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments
                            // .Include(e => e.CourseOffering)
                            .Include(e => e.StudentDetails)
                            .ToListAsync();
        }

        public async Task<Enrollment> AddAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<Enrollment> UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await  _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task DeleteAsync(int id)
        {
            var enrollment = await _context.Enrollments
                                // .Include(e => e.CourseOffering) // add only if you also want to delete CourseOFfering
                                .Include(e => e.StudentDetails)
                                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentDetailsIdAsync(string studentDetailsId)
        {
            return await  _context.Enrollments
                                                .Where(e => e.StudentDetailsId == studentDetailsId)
                                                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsByMonthAndYearAsync(int month, int year)
        {
            return await _context.Enrollments
                .Include(e => e.StudentDetails)
                .Where(e => e.EnrollmentDate.Month == month && e.EnrollmentDate.Year == year)
                .ToListAsync();
        }
    }
}