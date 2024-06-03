using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment> GetByIdAsync(int id);
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment> AddAsync(Enrollment enrollment);
        Task<Enrollment> UpdateAsync(Enrollment enrollment);
        Task DeleteAsync(int id);


        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentDetailsIdAsync(string studentDetailsId);
        Task<List<Enrollment>> GetEnrollmentsByMonthAndYearAsync(int month, int year);
    }
}