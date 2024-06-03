using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentRegisteration.Models;

namespace StudentRegisteration.ViewModels
{
    public class MonthlyReportViewModel
    {
        public List<Enrollment> Enrollments { get; set; }
        public double TotalCost { get; set; }
        public int SelectedMonth { get; set; }
        public int SelectedYear { get; set; }
        public string PaymentType { get; set; }
        public List<int> AvailableMonths { get; set; }
        public List<int> AvailableYears { get; set; }
    }
}