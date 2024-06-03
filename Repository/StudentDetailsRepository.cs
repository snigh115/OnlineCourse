using Microsoft.EntityFrameworkCore;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Repository
{
    public class StudentDetailsRepository : IStudentDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StudentDetails> GetByIdAsync(string id)
        {
            return await _context.StudentDetails.Include(s => s.User)
                                                .Include(s => s.Address)
                                                .Include(s => s.Enrollments)
                                                .FirstOrDefaultAsync(s => s.Id  == id);
        }

        public async Task<IEnumerable<StudentDetails>> GetAllAsync()
        {
            return await _context.StudentDetails
                    .Include(s => s.User)
                    .Include(s => s.Address)
                    .Include(s => s.Enrollments)
                    .ToListAsync();
        }

        public async Task<StudentDetails> AddAsync(StudentDetails studentDetails)
        {
            await _context.StudentDetails.AddAsync(studentDetails);
            await _context.SaveChangesAsync();
            return studentDetails;
        }

        public async Task<StudentDetails> UpdateAsync(StudentDetails studentDetails)
        {
            _context.StudentDetails.Update(studentDetails);
            await _context.SaveChangesAsync();
            return studentDetails;
        }

        public async Task DeleteAsync(string id)
        {
            var studentDetails = await _context.StudentDetails
                                    // .Include(s => s.User)    //  add if you also want to Delete User data
                                    // .Include(s => s.Address)  // add if you also want to delete related Address 
                                    .Include(s => s.Enrollments)
                                    .FirstOrDefaultAsync(s => s.Id == id);

            if (studentDetails != null)
            {
                _context.StudentDetails.Remove(studentDetails);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<StudentDetails> GetStudentDetailsByUserIdAsync(string userId)
        {
            return await _context.StudentDetails.FirstOrDefaultAsync(sd => sd.UserId == userId);
        }
    }
}