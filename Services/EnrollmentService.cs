using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository) => _enrollmentRepository = enrollmentRepository;

        public async Task<Enrollment> GetEnrollmentByIdAsync(int id)
        {
            return await _enrollmentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _enrollmentRepository.GetAllAsync();
        }

        public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
        {
            return await _enrollmentRepository.AddAsync(enrollment);
        }

        public async Task<Enrollment> UpdateEnrollmentAsync(Enrollment enrollment)
        {
            return await _enrollmentRepository.UpdateAsync(enrollment);
        }

        public  async Task DeleteEnrollmentAsync(int id)
        {
            await _enrollmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentDetailsIdAsync(string studentDetailsId)
        {
            return await _enrollmentRepository.GetEnrollmentsByStudentDetailsIdAsync(studentDetailsId);
        }

        public async Task<List<Enrollment>> GetEnrollmentsByMonthAndYearAsync(int month, int year)
        {
            return await _enrollmentRepository.GetEnrollmentsByMonthAndYearAsync(month, year);
        }
    }
}