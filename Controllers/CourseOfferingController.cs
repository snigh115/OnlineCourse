using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.ViewModels;

namespace StudentRegisteration.Controllers
{
    public class CourseOfferingController : Controller
    {
        private readonly ICourseOfferingService _courseOfferingService;
        private readonly ICourseService _courseService;

        public CourseOfferingController(ICourseOfferingService courseOfferingService, ICourseService courseService)
        {
            _courseOfferingService = courseOfferingService;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(CourseOfferingCreateViewModel viewModel)
        {
            var courseOfferings = await _courseOfferingService.GetAllCourseOfferingsAsync();
            var courses = await _courseService.GetAllCourseAsync();

            // implement Filter

            var conbinedData = from CourseOffering in courseOfferings
                                join Course in courses on CourseOffering.Id equals Course.CourseOfferingId
                                select new CourseOfferingCreateViewModel
                                {
                                    Id = CourseOffering.Id,
                                    AcademicYear = CourseOffering.AcademicYear,
                                    Semester = CourseOffering.Semester,
                                    CourseName = Course.Name,
                                };

            viewModel = new CourseOfferingCreateViewModel
            {
                CombinedData = conbinedData.ToList(),
            };
            
            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new CourseOfferingCreateViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseOfferingCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
               

                var courseOffering = new CourseOffering
                {
                    AcademicYear = viewModel.AcademicYear,
                    Semester = viewModel.Semester,
                    
                };

                await _courseOfferingService.CreateCourseOfferingAsync(courseOffering);

                int saveCourseOfferingId = courseOffering.Id;

                if (saveCourseOfferingId > 0 && !string.IsNullOrEmpty(viewModel.CourseName))
                {
                    var course = new Course
                    {
                        Name = viewModel.CourseName,
                        CourseOfferingId = saveCourseOfferingId,
                    };
                    await _courseService.CreateCourseAsync(course);
                }
                return RedirectToAction("Index");

            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var courseOffering = await _courseOfferingService.GetCourseOfferingByIdAsync(id);

            if (courseOffering == null)
            {
                return NotFound("there is no Course with that Id or There is No Id");
            }

            var course = await _courseService.GetCourseByCourseOfferingIdAsync(id);
            var courseName = course?.Name ?? "";

            var viewModel = new CourseOfferingCreateViewModel
            {
                AcademicYear = courseOffering.AcademicYear,
                Semester = courseOffering.Semester,
                CourseName = courseName,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseOfferingCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var courseOffering = await _courseOfferingService.GetCourseOfferingByIdAsync(id);

                courseOffering.AcademicYear = viewModel.AcademicYear;
                courseOffering.Semester = viewModel.Semester;

                if (!string.IsNullOrEmpty(viewModel.CourseName))
                {
                    var course = await _courseService.GetCourseByCourseOfferingIdAsync(id);

                    if (course == null)
                    {
                        course = new Course
                        {
                            CourseOfferingId = id,
                        };
                    }

                    course.Name = viewModel.CourseName;
                    await _courseService.UpdateCourseAsync(course);
                }

                try 
                {
                    await _courseOfferingService.UpdateCourseOfferingAsync(courseOffering);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating: " + ex.Message);
                }
            }
            return View(viewModel);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var courseOffering = await _courseOfferingService.GetCourseOfferingByIdAsync(id);

            if (courseOffering == null)
            {
                return NotFound("There is no Course Offering with that ID.");
            }

            try
            {
                var course = await _courseService.GetCourseByCourseOfferingIdAsync(id);
                if (course != null)
                {
                    await _courseService.DeleteCourseAsync(course.Id);
                }

                await _courseOfferingService.DeleteCourseOfferingAsync(courseOffering.Id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the Course Offering: " + ex.Message);
                return View("Error"); 
            }
        }
    }
}
