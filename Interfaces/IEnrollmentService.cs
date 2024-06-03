using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Enrollment> GetEnrollmentByIdAsync(int id);
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
        Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment);
        Task<Enrollment> UpdateEnrollmentAsync(Enrollment enrollment);
        Task DeleteEnrollmentAsync(int id);
        
        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentDetailsIdAsync(string studentDetailsId);
        Task<List<Enrollment>> GetEnrollmentsByMonthAndYearAsync(int month, int year);  
    }
}