// using Microsoft.AspNetCore.Mvc;
// using StudentRegisteration.Data;
// using StudentRegisteration.Interfaces;
// using StudentRegisteration.Models;
// using StudentRegisteration.ViewModels;

// namespace StudentRegisteration.Controllers;

// public class CoursesManagementController : Controller
// {
//     private readonly ApplicationDbContext _context;
//     private readonly ICourseService _courseService;

//     public CoursesManagementController(ApplicationDbContext context, ICourseService courseService)
//     {
//         _context = context;
//         _courseService = courseService;
//     }

//      private bool IsUserLoggedIn()
//     {
//         var userId = HttpContext.Session.GetString("IsLoggedIn");
//         return userId != null;
//     }

//     private bool IsAdmin()
//     {
//         var userId = HttpContext.Session.GetString("IsLoggedIn");
//         if (userId == null)
//         {
//             return false;
//         }

//         var user = _context.Users.Find(userId);
//         if (user == null)
//         {
//             ModelState.AddModelError("User", "No User Found!");
//             return false;
//         }

//         return user.Role == "Admin";
//     }

//     public IActionResult CourseList(string searchString)
//     {
//         if(!IsUserLoggedIn() ||!IsAdmin())
//         {
//             return RedirectToAction("Login","User");
//         }

//         var courses = _courseService.GetAllCourses();

//         if(!string.IsNullOrEmpty(searchString))
//         {
//             courses = courses.Where( c => 
//             c.Course.ToLower().Contains(searchString.ToLower()) ||
//             c.StudentYear.ToLower().Contains(searchString.ToLower()) ||
//             c.Semester.ToLower().Contains(searchString.ToLower()) ||
//             c.Cost.ToString().Contains(searchString) 
//             );
//         }

//         var viewModel = new CourseListViewModel
//         {
//             CourseOffering = courses,
//             ShowFullList = string.IsNullOrEmpty(searchString)
//         };
//         return View(viewModel);
//     }

//     public IActionResult Create()
//     {
//         if(!IsUserLoggedIn() || !IsAdmin())
//         {
//             return RedirectToAction("Login","User");
//         }

//         var CourseViewModel = new CourseViewModel();
//         return View(CourseViewModel);
//     }

//     [HttpPost]
//     public async Task<IActionResult> Create(CourseViewModel courseViewModel)
//     {
//         if(!IsUserLoggedIn() ||!IsAdmin())
//         {
//             return RedirectToAction("Login","User");
//         }

//         if(ModelState.IsValid)
//         {
//             var course = new CourseOffering
//             {
//                 Course = courseViewModel.Course,
//                 StudentYear = courseViewModel.StudentYear,
//                 Semester = courseViewModel.Semester, 
//                 Cost = courseViewModel.Cost
//             };

//             _courseService.CreateCourse(course);
//             return RedirectToAction("CourseList");
//         }

//         return View(courseViewModel);
//     }

//     public async Task<IActionResult> Edit(int id)
//     {
//         if(!IsUserLoggedIn() ||!IsAdmin())
//         {
//             return RedirectToAction("Login","User");
//         }
//         var course = await _courseService.GetCourseById(id);
//         if (course == null)
//         {
//             return NotFound();
//         }

//         var courseViewModel = new CourseViewModel
//         {
//             Id = course.Id,
//             Course = course.Course,
//             StudentYear = course.StudentYear,
//             Semester = course.Semester,
//             Cost = course.Cost
            
//         };

//         return View (courseViewModel);
//     }

//     [HttpPost]
//     public async Task<IActionResult> Edit(int id,CourseViewModel courseViewModel)
//     {
//         if(!IsUserLoggedIn() ||!IsAdmin())
//         {
//             return RedirectToAction("Login","User");
//         }

//         if (ModelState.IsValid)
//         {
//             var course = await _courseService.GetCourseById(id);
//             if (course == null)
//             {
//                 return NotFound();
//             }

//             course.Course = courseViewModel.Course;
//             course.StudentYear = courseViewModel.StudentYear;
//             course.Semester = courseViewModel.Semester;
//             course.Cost = courseViewModel.Cost;

//             await _courseService.UpdateCourse(course);
//             return RedirectToAction("CourseList");
//         }

//         return View(courseViewModel);
//     }

//     public async Task<IActionResult> Delete(int id)
//     {
//         if(!IsUserLoggedIn() ||!IsAdmin())
//         {
//             return RedirectToAction("Login","User");
//         }

//         var course = await _courseService.GetCourseById(id);
//         if (course == null)
//         {
//             return NotFound();
//         }

//         await _courseService.DeleteCourse(id);

//         return RedirectToAction("CourseList");
//     }
// }