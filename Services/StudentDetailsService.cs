using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.Repository;

namespace StudentRegisteration.Services
{
    public class StudentDetailsService : IStudentDetailsService
    {
        private readonly IStudentDetailsRepository _studentDetailsRepository;
        private readonly IWebHostEnvironment _env;

        public StudentDetailsService(IStudentDetailsRepository studentDetailsRepository, IWebHostEnvironment env)
        {
            _studentDetailsRepository = studentDetailsRepository;
            _env = env;
        }

        public async Task<StudentDetails> GetStudentDetailsByIdAsync(string id)
        {
            return await _studentDetailsRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<StudentDetails>> GetAllStudentDetailsAsync()
        {
            return await _studentDetailsRepository.GetAllAsync();
        }

        public async Task<StudentDetails> GetStudentDetailsByUserIdAsync(string userId)
        {
            return await _studentDetailsRepository.GetStudentDetailsByUserIdAsync(userId);
        }

        public async Task<StudentDetails> AddStudentDetailsAsync(StudentDetails studentDetails, IFormFile nrcFrontPic, IFormFile nrcBackPic, IFormFile profilePic)
        {
            await HandleImageUploads(studentDetails, nrcFrontPic, nrcBackPic, profilePic);
            
            studentDetails = await _studentDetailsRepository.AddAsync(studentDetails);

            return studentDetails;
        }

        public async Task<StudentDetails> UpdateStudentDetailsAsync(StudentDetails studentDetails, IFormFile nrcFrontPic, IFormFile nrcBackPic, IFormFile profilePic)
        {
            await HandleImageUploads(studentDetails, nrcFrontPic, nrcBackPic, profilePic);

            studentDetails = await _studentDetailsRepository.UpdateAsync(studentDetails);

            return studentDetails;
        }

        public async Task DeleteStudentDetails(string id)
        {
            var studentDetails = await _studentDetailsRepository.GetByIdAsync(id);

            if(studentDetails == null )
            {
                throw new InvalidOperationException($"StudentDetails with id: {id} not found");
            }

            if(!string.IsNullOrEmpty(studentDetails.NrcFrontPicPath))
            {
                await DeleteFileAsync(studentDetails.NrcFrontPicPath);
            }

            if(!string.IsNullOrEmpty(studentDetails.NrcBackPicPath))
            {
                await DeleteFileAsync(studentDetails.NrcBackPicPath);
            }

            if(!string.IsNullOrEmpty(studentDetails.ProfilePicPath))
            {
                await DeleteFileAsync(studentDetails.ProfilePicPath);
            }
            
            await _studentDetailsRepository.DeleteAsync(id);
        }

        private async Task HandleImageUploads(StudentDetails studentDetails, IFormFile nrcFrontPic, IFormFile nrcBackPic, IFormFile profilePic)
        {
            if (_env.WebRootPath == null)
            {
                throw new InvalidOperationException("WebRoothPath is not set");
            }

            string uploadsFolder = "StudentImages";

            if (nrcFrontPic != null && nrcFrontPic.Length > 0 )
            {
                if (!IsImageFile(nrcFrontPic))
                {
                    throw new InvalidOperationException("Invalid file format for NRC Front Pic. Only image files are allowed.");
                }
                studentDetails.NrcFrontPicPath = await SaveFileAsync(nrcFrontPic, uploadsFolder);
            }

            if (nrcBackPic != null && nrcBackPic.Length > 0)
            {
                if (!IsImageFile(nrcBackPic))
                {
                    throw new InvalidOperationException("Invalid file format for NRC Back Pic. Only image files are allowed.");
                }
                studentDetails.NrcBackPicPath = await SaveFileAsync(nrcBackPic, uploadsFolder);
            }

            if (profilePic != null && profilePic.Length > 0)
            {
                if (!IsImageFile(profilePic))
                {
                    throw new InvalidOperationException("Invalid file format for Profile Pic. Only image files are allowed.");
                }
                studentDetails.ProfilePicPath = await SaveFileAsync(profilePic, uploadsFolder);
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string uploadsFolder)
        {
            string uploadsPath = Path.Combine(_env.WebRootPath, uploadsFolder);
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            string fileName = GenerateUniqueFileName(file.FileName);
            string filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("/", uploadsFolder, fileName).Replace("\\","/");
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty");
            }

            if (_env.WebRootPath == null)
            {
                throw new InvalidOperationException("WebRootPath is not set.");
            }

            string fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            else
            {
                throw new FileNotFoundException("File not found.", fullPath);
            }
        }

        private bool IsImageFile(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            return allowedExtensions.Contains(fileExtension);
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            return Guid.NewGuid() + Path.GetExtension(originalFileName);
        }
    }
}