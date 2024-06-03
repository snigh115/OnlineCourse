using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface IStudentDetailsRepository
    {
        Task<StudentDetails> GetByIdAsync(string id);
        Task<IEnumerable<StudentDetails>> GetAllAsync();
        Task<StudentDetails> AddAsync(StudentDetails studentDetails);
        Task<StudentDetails> UpdateAsync(StudentDetails studentDetails);
        Task DeleteAsync(string id);

        Task<StudentDetails> GetStudentDetailsByUserIdAsync(string userId);
    }
}