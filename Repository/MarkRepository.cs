using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Repository
{
    public class MarkRepository : IMarkRepository
    {
        private readonly ApplicationDbContext _context;

        public MarkRepository(ApplicationDbContext context) => _context = context;

        public async Task<Mark> GetByIdAsync(int id)
        {
            return await _context.Marks.FindAsync(id);
        }

        public async Task<IEnumerable<Mark>> GetAllAsync()
        {
            return await _context.Marks.ToListAsync();
        }

        public async Task CreateAsync(Mark mark)
        {
            await _context.Marks.AddAsync(mark);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Mark mark)
        {
            _context.Marks.Update(mark);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mark = await GetByIdAsync(id);
            if (mark != null)
            {
                _context.Marks.Remove(mark);
                await _context.SaveChangesAsync();
            }
        }

        // public async Task<IEnumerable<Mark>> GetMarkByEnrollmentIdAsync(int enrollmentId)
        // {
        //     var marks = await _context.EnrollmentCourses
        //         .Where(ec => ec.EnrollmentId == enrollmentId)
        //         .Select(ec => ec.Mark)
        //         .ToListAsync();

        //     return marks;
        // }

        public async Task<Mark> GetMarkByCourseIdAndEnrollmentIdAsync(int courseId,int enrollmentId)
        {
             var mark =  await _context.EnrollmentCourses
                    .Where(ec => ec.CourseId == courseId && ec.EnrollmentId == enrollmentId)
                    .Select(ec => ec.Mark)
                    .FirstOrDefaultAsync();
            return mark;
        }
    }
}