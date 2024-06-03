// using Microsoft.EntityFrameworkCore;
// using StudentRegisteration.Data;
// using StudentRegisteration.Interfaces;
// using StudentRegisteration.Models;
// using StudentRegisteration.ViewModels;

// namespace StudentRegisteration.Services;

// public class StudentService : IStudentService
// {
//     private readonly ApplicationDbContext _context;

//     public StudentService(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     public StudentDetails GetStudentDetails(string studentId)
//     {
//         return _context.StudentDetails
//             .Include(sd => sd.Registerations)
//             .ThenInclude(r => r.CourseOffering)
//             .FirstOrDefault(sd => sd.Id == studentId);
//     }

//     public List<CourseOffering> GetAllCourse()
//     {
//         return _context.CourseOfferings.ToList();
//     }

//     public List<CourseYearViewModel> GetAvailableCourseByYear()
//     {
//         var courses = _context.CourseOfferings.ToList();

//         return courses.GroupBy(co => co.AcademicYear)
//             .Select(group => new CourseYearViewModel
//             {
//                 Year = group.Key,
//                 Courses = group.ToList()
//             }).ToList();
//     }
//     // public List<CourseYearViewModel> GetAvailableCourse()
//     // {
//     //     // var studentDetails = GetStudentDetails(studentId);
        
//     //     // return _context.CourseOfferings.ToList();

//     //     var courses = _context.CourseOfferings.ToList();  

//     //     return courses.GroupBy(co => co.StudentYear)
//     //         .Select(group => new CourseYearViewModel
//     //         {
//     //             Year = group.Key,
//     //             Courses = group.ToList()
//     //         })
//     //         .ToList();
            
//     // }
// }