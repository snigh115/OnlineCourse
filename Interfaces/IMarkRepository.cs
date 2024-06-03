using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface IMarkRepository
    {
        Task<Mark> GetByIdAsync(int id);
        Task<IEnumerable<Mark>> GetAllAsync();
        Task CreateAsync(Mark mark);
        Task UpdateAsync(Mark mark);
        Task DeleteAsync(int id);
        Task<Mark> GetMarkByCourseIdAndEnrollmentIdAsync(int courseId,int enrollmentId);
    }
}