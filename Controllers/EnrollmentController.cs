using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.ViewModels;

namespace StudentRegisteration.Controllers
{
    public class EnrollmentController : Controller
    {

        private readonly IEnrollmentService _enrollmentService;
        private readonly IStudentDetailsService _studentDetailsService;
        private readonly ICourseOfferingService _courseOfferingService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentCourseService _enrollmentCourseService;
        private readonly IMarkService _markService;
     

        public EnrollmentController(IEnrollmentService enrollmentService,
                                    IStudentDetailsService studentDetailsService,
                                    ICourseOfferingService courseOfferingService,
                                    ICourseService courseService,
                                    IEnrollmentCourseService enrollmentCourseService,
                                    IMarkService markService
                                    )
        {
            _enrollmentService = enrollmentService;
            _studentDetailsService = studentDetailsService;
            _courseOfferingService = courseOfferingService;
            _courseService = courseService;
            _enrollmentCourseService = enrollmentCourseService;
            _markService = markService;
           
        }

    private bool IsUserInRole(string role)
    {
        var loggedInUserString = HttpContext.Session.GetString("loggedInUser");
        if(loggedInUserString == null)
        {
            return false;
        }

        // User loggedInUser;
        try 
        {
            var loggedInUser = JsonSerializer.Deserialize<User>(loggedInUserString);
            return loggedInUser.Role == role;
        }
        catch(JsonException ex)
        {
                // Log the exception
            Console.WriteLine($"Error deserializing user data: {ex.Message}");

            return false;
        }          
    }

    private User GetLoggedInUser()
    {
        var loggedInUserString = HttpContext.Session.GetString("loggedInUser");

        if (loggedInUserString != null)
        {
            try
            {
                var loggedInUser = JsonSerializer.Deserialize<User>(loggedInUserString);
                return loggedInUser;
            }
            catch(JsonException ex)
            {
                Console.WriteLine($"Error deserializing user data: {ex.Message}");

                return null;
            }
        }
        else
        {
            return null;
        }
    }

        [HttpGet]
        public async Task<ActionResult> MonthlyPaymentSheet(int month = 0, int year = 0)
        {
            // default to current month and year if not specified
            if (month == 0 || year == 0)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            var enrollments = await _enrollmentService.GetEnrollmentsByMonthAndYearAsync(month, year);

            var totalCost = enrollments.Sum(e => e.Cost);

            var viewModel = new MonthlyReportViewModel
            {
                Enrollments = enrollments,
                TotalCost = totalCost,
                SelectedMonth = month,
                SelectedYear = year,
                AvailableMonths = Enumerable.Range(1, 12).ToList(),
                AvailableYears = Enumerable.Range(DateTime.Now.Year - 10, 11).ToList(),
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> MarkList(int page = 1, int pageSize = 10, string courseName = "", string academicYear = "", string semester = "")
        {
            var enrollmentCourses = await _enrollmentCourseService.GetAllEnrollmentCoursesAsync();
            var courseNames = await _enrollmentCourseService.GetUniqueCourseNamesAsync();
        // filter 
            if (!string.IsNullOrEmpty(courseName))
            {
                enrollmentCourses = enrollmentCourses.Where(ec => ec.Course.Name == courseName).ToList();
                // courseNames = new List<string> { courseName };
            }
            if (!string.IsNullOrEmpty(academicYear))
            {
                enrollmentCourses = enrollmentCourses.Where(ec => ec.Course.CourseOfferings.AcademicYear == academicYear).ToList();
            }
            if (!string.IsNullOrEmpty(semester))
            {
                enrollmentCourses = enrollmentCourses.Where(ec => ec.Course.CourseOfferings.Semester == semester).ToList();
            }

            // var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            var enrollments = enrollmentCourses.Select(ec => ec.Enrollment).ToList();
 
            var studentMarkList = new List<StudentMarksViewModel>();

            var totalCount = enrollments.Count();
            var totalPages = (int)Math.Ceiling((decimal) totalCount / pageSize);

            var paginatedEnrollment = enrollments
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            foreach (var enrollment in paginatedEnrollment)
            {
                var studentDetails = enrollment.StudentDetails;
                var enrollmentCoursesForEnrollment  = await _enrollmentCourseService.GetEnrollmentCourseByEnrollmentId(enrollment.Id);


                foreach (var enrollmentCourse in enrollmentCoursesForEnrollment )
                {
                    var course = enrollmentCourse.Course;
                    var courseOffering = course.CourseOfferings;
                    var mark = enrollmentCourse.Mark;

                    
                        var StudentMarkViewModel = new StudentMarksViewModel
                        {
                            StudentName = studentDetails.Name,
                            RollNumber = enrollment.RollNumber,
                            CourseName = course.Name,
                            AcademicYear = courseOffering.AcademicYear,
                            Semester = courseOffering.Semester,
                            MarkValue = mark?.MarkValue,
                            IsPassed = mark?.IsPassed,
                        };

                        studentMarkList.Add(StudentMarkViewModel);
                    
                }
            }

            var viewModel = new StudentMarksListViewModel
            {
                StudentMarks = studentMarkList,
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = totalCount,
                TotalPages = totalPages,
                FilterOptionsForMark = new FilterOptionsForMarkViewModel
                {
                    CourseNames = courseNames,
                    CourseName = courseName,
                    AcademicYear = academicYear,
                    Semester = semester,
                }
            };

            return View(viewModel);
        }
  
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = GetLoggedInUser();
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var studentDetails = await _studentDetailsService.GetStudentDetailsByUserIdAsync(user.Id);
            if (studentDetails == null)
            {
                return RedirectToAction("Error");
            }

            var enrollments = await _enrollmentService.GetEnrollmentsByStudentDetailsIdAsync(studentDetails.Id);
            if (enrollments == null || !enrollments.Any())
            {
                // Handle scenario where no enrollments are found
                return RedirectToAction("NoEnrollments");
            }

            var enrollmentCourses = new List<EnrollmentCourseViewModel>();

            foreach (var enrollment in enrollments)
            {
                var courses = await _enrollmentCourseService.GetEnrollmentCourseByEnrollmentId(enrollment.Id);
            

                foreach (var course in courses)
                {
                    var markValue = await _markService.GetMarkByCourseIdAndEnrollmentIdAsync(course.CourseId, course.EnrollmentId);

                    var enrollmentCourseViewModel = new EnrollmentCourseViewModel
                    {
                        StudentName = enrollment.StudentDetails.Name,
                        StudentDetailsId = enrollment.StudentDetailsId,
                        RollNumber = enrollment.RollNumber,
                        EnrollmentId = enrollment.Id,
                        CourseId = course.CourseId,
                        CourseName = course.Course.Name,
                        AcademicYear = course.Course.CourseOfferings.AcademicYear,
                        Semester = course.Course.CourseOfferings.Semester,
                        IsCompleted = course.Enrollment.Completed,
                        Status = course.Enrollment.Status,
                        MarkValue =  markValue?.MarkValue,
                        IsPassed = markValue?.IsPassed,
                        // Include MarkValue and IsPassed properties here if needed
                    };

                    enrollmentCourses.Add(enrollmentCourseViewModel);
                }
            }
            return View(enrollmentCourses);
        }


// for admin to view entire StudentDetails 
        [HttpGet]
        public async Task<IActionResult> StudentAndEnrollmentList(int page = 1,int pageSize = 10,string courseName = "", string academicYear = "", string semester = "", string status = "", bool? isPassed = null, string studentName = "", string rollNumber = "")
        {
            if(!IsUserInRole("Admin"))
            {
                return RedirectToAction("Login","User");
            }

            var courseNames = await _enrollmentCourseService.GetUniqueCourseNamesAsync();

            var enrollmentCourse = await _enrollmentCourseService.GetAllEnrollmentCoursesAsync();

            // filter 

            if(!string.IsNullOrEmpty(courseName))
            {
                enrollmentCourse = enrollmentCourse.Where(ec => ec.Course.Name == courseName).ToList();
            }
            if(!string.IsNullOrEmpty(academicYear))
            {
                enrollmentCourse = enrollmentCourse.Where(ec => ec.Course.CourseOfferings.AcademicYear == academicYear).ToList();   
            }
            if(!string.IsNullOrEmpty(semester))
            {
                enrollmentCourse = enrollmentCourse.Where(ec => ec.Course.CourseOfferings.Semester == semester).ToList();   
            }
            if(!string.IsNullOrEmpty(status))
            {
                enrollmentCourse = enrollmentCourse.Where(ec => ec.Enrollment.Status == status).ToList();
            }
            if(isPassed.HasValue)
            {
                enrollmentCourse = enrollmentCourse.Where(ec => ec.Mark != null && ec.Mark.IsPassed == isPassed).ToList();
            }

            // use .ToLower() and .Contains() methods to search just minimum data
            if(!string.IsNullOrEmpty(studentName))
            {
                studentName = studentName.ToLower();
                enrollmentCourse = enrollmentCourse.Where(ec => ec.Enrollment.StudentDetails.Name.ToLower().Contains(studentName)).ToList();
            }
            if(!string.IsNullOrEmpty(rollNumber))
            {
                rollNumber = rollNumber.ToLower();
                enrollmentCourse = enrollmentCourse.Where(ec => ec.Enrollment.RollNumber.ToLower().Contains(rollNumber)).ToList();
            }


            var distinctEnrollmentCourses = enrollmentCourse.GroupBy(ec => ec.EnrollmentId).Select(group => group.First());

            var totalCount = distinctEnrollmentCourses.Count();
            var totalPages = (int)Math.Ceiling((decimal) totalCount / pageSize);

            // apply pagination
            var paginatedEnrollment = enrollmentCourse
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

            var viewModel = new EnrollmentListViewModel
            {
                enrollmentCourseViewModels = paginatedEnrollment.Select(ec => new EnrollmentCourseViewModel
                {
                    StudentName = ec.Enrollment?.StudentDetails?.Name ?? "Unknown",
                    StudentDetailsId = ec.Enrollment?.StudentDetails?.Id ?? "",
                    RollNumber = ec.Enrollment?.RollNumber ?? "",
                    EnrollmentId = ec.EnrollmentId,
                    CourseId = ec.CourseId,
                    CourseName = ec.Course?.Name ?? "",
                    AcademicYear = ec.Course?.CourseOfferings?.AcademicYear ?? "",
                    Semester = ec.Course?.CourseOfferings?.Semester ?? "",
                    IsCompleted = ec.Enrollment?.Completed ?? false,
                    IsPassed = ec.Mark?.IsPassed,
                    Status = ec.Enrollment?.Status ?? "",
                    Page = page,
                    TotalCount = totalCount,
                    AvailableStatuses = new List<string> {"Pending", "Approved", "Rejected"}
                    
                }).ToList(),
                FilterOptions = new EnrollmentFilterOptionsViewModel
                {
                    CourseName = courseName,
                    AcademicYear = academicYear,
                    Semester = semester,
                    Status = status,
                    IsPassed = isPassed,
                    CourseNames = courseNames,
                    StudentName = studentName,
                    RollNumber = rollNumber,
                },

                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = totalCount,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEnrollmentStatus(int enrollmentId, string status)
        {
            if (!IsUserInRole("Admin"))
            {
                return RedirectToAction("Login", "User");
            }

            try
            {
                await _enrollmentCourseService.UpdateEnrollmentStatusAsync(enrollmentId, status);
                // Json(new {success = true, message = "Status updated successfully."});
                return RedirectToAction("StudentAndEnrollmentList");
            }
             catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to update status. Error: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int enrollmentId)
        {
            if(enrollmentId == null)
            {
                return NotFound("No Id found for Enrollment");
            }

            var enrollmentCourse = await _enrollmentCourseService.GetEnrollmentCourseByEnrollmentId(enrollmentId);
            if (enrollmentCourse == null || !enrollmentCourse.Any()) 
            {
                return NotFound("No Related Enrollment for that Id");
            }

            var firstEnrollmentCourse = enrollmentCourse.First();

            var viewModel = new EnrollmentCourseDetailsViewModel();

            if(firstEnrollmentCourse != null && 
                firstEnrollmentCourse.Enrollment != null &&
                firstEnrollmentCourse.Enrollment.StudentDetails != null && 
                firstEnrollmentCourse.Enrollment.StudentDetails.User != null &&
                firstEnrollmentCourse.Course != null &&
                firstEnrollmentCourse.Course.CourseOfferings != null)
            {
                viewModel.Name = firstEnrollmentCourse.Enrollment.StudentDetails.Name;
                viewModel.Email = firstEnrollmentCourse.Enrollment.StudentDetails.User.Email;
                viewModel.NrcNumber = firstEnrollmentCourse.Enrollment.StudentDetails.NrcNumber;
                viewModel.PhoneNumber = firstEnrollmentCourse.Enrollment.StudentDetails.PhoneNumber;
                viewModel.AcademicYear = firstEnrollmentCourse.Course.CourseOfferings.AcademicYear;
                viewModel.Semester = firstEnrollmentCourse.Course.CourseOfferings.Semester;

                viewModel.CourseName = enrollmentCourse.Select(ec => ec.Course.Name).ToList();
                viewModel.Mark = enrollmentCourse.Select(ec => ec.Mark?.MarkValue).ToList();   
                viewModel.IsPassed = enrollmentCourse.Select(ec => ec.Mark?.IsPassed).ToList();

                viewModel.PaymentType = firstEnrollmentCourse.Enrollment.PaymentType;  

                viewModel.Cost = firstEnrollmentCourse.Enrollment.Cost;
                viewModel.TechnicalFee = viewModel.Cost * (15.0 / 100);
                viewModel.TransitionFee = viewModel.Cost * (10.0 / 100);
                viewModel.ActualCost = viewModel.Cost - (viewModel.TechnicalFee  + viewModel.TransitionFee);    

                viewModel.Status = firstEnrollmentCourse.Enrollment.Status;

                viewModel.RollNumber = firstEnrollmentCourse.Enrollment.RollNumber;
                viewModel.ProfilePicPath = firstEnrollmentCourse.Enrollment.StudentDetails.ProfilePicPath;
                       
            }

            return View(viewModel);
        }

        

        [HttpGet]
        public IActionResult GetDistinctCourseNamesAsyncByAcademicYearAndSemester(string academicYear, string semester)
        {
            var courseNames = _courseOfferingService.GetDistinctCourseNamesAsyncByAcademicYearAndSemester(academicYear, semester).Result;
            return Json(courseNames);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string academicYear = "" , string semester = "", string courseName = "")
        {
            IEnumerable<string> courseNames;

            if (!string.IsNullOrEmpty(academicYear) && !string.IsNullOrEmpty(semester))
            {
                courseNames = await _courseOfferingService.GetDistinctCourseNamesAsyncByAcademicYearAndSemester(academicYear, semester);
            }
            else
            {
                courseNames = await _courseOfferingService.GetDistinctCourseNamesAsync();
            }

            var viewModel = new EnrollmentCreateViewModel
            {
                AcademicYears = await _courseOfferingService.GetDistinctAcademicYearsAsync(),
                Semesters = await _courseOfferingService.GetDistinctSemestersAsync(),
                CourseNames = courseNames.Any() ? courseNames : Enumerable.Empty<string>(),

                AcademicYear = academicYear,
                Semester = semester, 

              
                
                CourseName = !string.IsNullOrEmpty(courseName) ? new List<string> {courseName} : new List<string>(),

            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentCreateViewModel viewModel)
        {
            var loggedInUser = GetLoggedInUser();
            var studentDetails = await _studentDetailsService.GetStudentDetailsByUserIdAsync(loggedInUser.Id);

          
  // var studentPassed = viewModel.MarkValue >= 50;

            if(!ModelState.IsValid)
            {
                var enrollment = new Enrollment
                {
                    RollNumber = await GenerateRollNumber(viewModel.AcademicYear, viewModel.Semester),
                    Status = viewModel.Status,
                    EnrollmentDate = DateTime.Now,
                    ApprovalDate = DateTime.Now, // TODO: Format this
  
                    StudentDetailsId = studentDetails.Id,
                    Cost = viewModel.Cost,
                    PaymentType = viewModel.PaymentType,
                    Completed = viewModel.Completed,
              
                    // MarkValue = viewModel.MarkValue,
                    // IsPassed = studentPassed
                    
                };

                await _enrollmentService.AddEnrollmentAsync(enrollment);        

                foreach (var courseName in viewModel.SelectedCourses)
                {
                   if (viewModel.MarkValues.ContainsKey(courseName))
                   {

                        var markValue = viewModel.MarkValues[courseName];
                        var isPassed = markValue >= 45;

                        var mark = new Mark
                        {
                            MarkValue = markValue,
                            IsPassed = isPassed,
                        };

                        await _markService.CreateMarkAsync(mark);

                        var courseId = await _courseService.GetCourseIdByFilteringAsync(viewModel.AcademicYear, viewModel.Semester, courseName);
                        if (courseId != null)
                        {
                            var enrollmentCourse = new EnrollmentCourse
                            {
                                EnrollmentId = enrollment.Id,
                                CourseId = courseId.Value,
                                MarkId = mark.Id,
                            };

                            await _enrollmentCourseService.AddEnrollmentCourseAsync(enrollmentCourse);
                        }
                   }
                }

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
// V1.3
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create(EnrollmentCreateViewModel viewModel)
        // {
        //     var loggedInUser = GetLoggedInUser();
        //     var studentDetails = await _studentDetailsService.GetStudentDetailsByUserIdAsync(loggedInUser.Id);
        //     Console.WriteLine(viewModel.CourseName);
        //     Console.WriteLine(viewModel.CourseNames);
        //     if(!ModelState.IsValid)
        //     {
        //         var enrollment = new Enrollment
        //         {
        //             RollNumber = GenerateRollNumber(viewModel.AcademicYear, viewModel.Semester),
        //             Status = viewModel.Status,
        //             EnrollmentDate = DateTime.Now,
        //             ApprovalDate = DateTime.Now, // TODO: Format this
  
        //             StudentDetailsId = studentDetails.Id,
        //             Cost = viewModel.Cost,
        //             PaymentType = viewModel.PaymentType,
        //             Completed = viewModel.Completed
        //         };

        //         await _enrollmentService.AddEnrollmentAsync(enrollment);

        //         // Populate the course IDs directly in the Enrollment
        //         foreach (var courseName in viewModel.CourseName)
        //         {
        //             var courseId = await _courseService.GetCourseIdByFilteringAsync(viewModel.AcademicYear, viewModel.Semester, courseName);
        //             if (courseId != null)
        //             {
        //                 var course = await _courseService.GetCourseByIdAsync(courseId.Value);

        //                 if (course != null)
        //                 {
        //                     course.EnrollmentId = enrollment.Id;

        //                     await _courseService.UpdateCourseAsync(course);

                            
        //                 }
        
        //             }
        //         }

        //         return RedirectToAction("Index");
        
        //     }

        //     return View(viewModel);
        // }

        public async Task<IActionResult> AddMark()
        {
            var user = GetLoggedInUser();
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var studentDetails = await _studentDetailsService.GetStudentDetailsByUserIdAsync(user.Id);
            var enrollments = await _enrollmentService.GetEnrollmentsByStudentDetailsIdAsync(studentDetails.Id);
            
            // var enrollmentCourses = await _enrollmentCourseService.GetEnrollmentCourseByEnrollmentId(enrollments.Id);
            var enrollmentCourses = new List<EnrollmentCourseViewModel>();

            foreach (var enrollment in enrollments)
            {
                var courses = await _enrollmentCourseService.GetEnrollmentCourseByEnrollmentId(enrollment.Id);

                foreach (var course in courses)
                {
                    enrollmentCourses.Add(new EnrollmentCourseViewModel
                    {
                        StudentName = enrollment.StudentDetails.Name,
                        StudentDetailsId = enrollment.StudentDetailsId,
                        RollNumber = enrollment.RollNumber,
                        EnrollmentId = enrollment.Id,
                        CourseId = course.CourseId,
                        CourseName = course.Course.Name,
                        AcademicYear = course.Course.CourseOfferings.AcademicYear,
                        Semester = course.Course.CourseOfferings.Semester,
                        IsCompleted = course.Enrollment.Completed,
                        Status = course.Enrollment.Status,
                        // MarkValue = course.Enrollment.MarkValue,
                        // IsPassed = course.Enrollment.IsPassed,    
                    });
                }

               
            }
             return View(enrollmentCourses);
        }

        public async Task<IActionResult> EnrollmentCounts()
        {
            var academicYears = await _courseOfferingService.GetDistinctAcademicYearsAsync();
            var semesters = await _courseOfferingService.GetDistinctSemestersAsync();

            var enrollmentCounts = new List<EnrollmentCountViewModel>();
            var totalCounts = await _enrollmentCourseService.GetTotalEnrollmentCountsAsync();

            foreach (var academicYear in academicYears)
            {
                
                    var enrollmentCount = await _enrollmentCourseService.GetEnrollmentCountByAcademicYearAsync(academicYear);  
                
                
                    var uniqueEnrollmentCount = await _enrollmentCourseService.GetUniqueEnrollmentCountByAcademicYearAsync(academicYear);

                
                    var enrollmentCountViewModel = new EnrollmentCountViewModel
                    {
                        
                        AcademicYear = academicYear,
                        EnrollmentCount = enrollmentCount,
                        UniqueEnrollmentCount = uniqueEnrollmentCount,
                        
                    };

                    enrollmentCounts.Add(enrollmentCountViewModel);
                
                
            }

            var viewModel = new EnrollmentTotalCountsViewModel
            {
                EnrollmentCountViewModel = enrollmentCounts,
                TotalCount = totalCounts,
            };

            return View(viewModel);
        }


       

        private async Task<string> GenerateRollNumber(string academicYear, string semester)
        {
            int academicYearNumber = GetAcademicYearNumber(academicYear);
            int semesterNumber = GetSemesterNumber(semester);
            int enrollmentCount = await _enrollmentCourseService.GetEnrollmentCountByAcademicYearAndSemesterAsync(academicYear, semester);

            string rollNumber = $"{academicYearNumber}{semesterNumber}-{enrollmentCount + 1}";

            return rollNumber;
        }

        private int GetAcademicYearNumber(string academicYear)
        {
            switch (academicYear)
            {
                case "1st Year":
                    return 1;
                case "2nd Year":
                    return 2;
                case "3rd Year":
                    return 3;
                default:
                    throw new ArgumentException("Invalid academic year.");
            }
        }

        private int GetSemesterNumber(string semester)
        {
            switch (semester)
            {
                case "FirstSemester":
                    return 1;
                case "SecondSemester":
                    return 2;
                default:
                    throw new ArgumentException("Invalid semester.");
            }
        }

    }
}