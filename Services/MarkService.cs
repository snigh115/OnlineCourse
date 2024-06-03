using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;

namespace StudentRegisteration.Services
{
    public class MarkService : IMarkService
    {
        private readonly IMarkRepository _markRepository;

        public  MarkService(IMarkRepository markRepository)
        {
            _markRepository = markRepository;
        }

        public async Task<Mark> GetMarkByIdAsync(int id)
        {
            return await _markRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Mark>> GetAllMarksAsync()
        {
            return await _markRepository.GetAllAsync();
        }

        public async Task CreateMarkAsync(Mark mark)
        {
            await _markRepository.CreateAsync(mark);
        }

        public async Task UpdateMarkAsync(Mark mark)
        {
            await _markRepository.UpdateAsync(mark);
        }

        public async Task DeleteMarkAsync(int id)
        {
            await _markRepository.DeleteAsync(id);
        }

        public async Task<Mark> GetMarkByCourseIdAndEnrollmentIdAsync(int courseId,int enrollmentId)
        {
           return await _markRepository.GetMarkByCourseIdAndEnrollmentIdAsync(courseId,enrollmentId);
        }
    }
}