using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegisteration.ViewModels
{
    public class StudentMarksViewModel
    {
        public string StudentName { get; set; }
        public string RollNumber { get; set; }
        public string CourseName { get; set; }
        public int? MarkValue { get; set; } // can be null
        public bool? IsPassed { get; set; } // can be null
        public string AcademicYear { get; set; }
        public string Semester { get; set; }
    }

    public class FilterOptionsForMarkViewModel()
    {
        public string CourseName { get; set; }
        public List<string> CourseNames { get; set;}
        public string AcademicYear { get; set; }
        public string Semester { get; set; }
    }

    public class StudentMarksListViewModel
    {
        public List<StudentMarksViewModel> StudentMarks { get; set; }
        public FilterOptionsForMarkViewModel FilterOptionsForMark { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}