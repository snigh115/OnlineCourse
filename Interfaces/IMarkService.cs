using StudentRegisteration.Models;

namespace StudentRegisteration.Interfaces
{
    public interface IMarkService
    {
        Task<Mark> GetMarkByIdAsync(int id);
        Task<IEnumerable<Mark>> GetAllMarksAsync();
        Task CreateMarkAsync(Mark mark);
        Task UpdateMarkAsync(Mark mark);
        Task DeleteMarkAsync(int id);

        Task<Mark> GetMarkByCourseIdAndEnrollmentIdAsync(int courseId,int enrollmentId);
    }
}