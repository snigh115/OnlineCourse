using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface IStudentDetailsService
    {
        Task<StudentDetails> GetStudentDetailsByIdAsync(string id);
        Task<IEnumerable<StudentDetails>> GetAllStudentDetailsAsync();
        Task<StudentDetails> AddStudentDetailsAsync(StudentDetails studentDetails, IFormFile nrcFrontPic, IFormFile nrcBackPic, IFormFile profilePic);
        Task<StudentDetails> UpdateStudentDetailsAsync(StudentDetails studentDetails, IFormFile nrcFrontPic, IFormFile nrcBackPic, IFormFile profilePic);
        Task DeleteStudentDetails(string Id);

        Task<StudentDetails> GetStudentDetailsByUserIdAsync(string userId);

        Task<string> SaveFileAsync(IFormFile file, string uploadsFolder);


    }
}