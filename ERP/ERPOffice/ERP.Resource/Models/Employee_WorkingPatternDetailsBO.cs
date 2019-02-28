using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERP.Resource.Models
{
    public class Employee_WorkingPatternDetailsBO
    {
        public int WorkingPatternID { get; set; }
        public int EmployeeID { get; set; }

        [Display(Name = "Change Date"), Required]
        public DateTime? ChangeDate { get; set; }

        [Display(Name = "Working Hours"), Required]
        [Range(typeof(double), "1", "12")]
        public double? NoOfWorkingHours { get; set; }

        [Display(Name = "Sun")]
        public bool Sunday { get; set; }

        [Display(Name = "Mon")]
        public bool Monday { get; set; }

        [Display(Name = "Tue")]
        public bool Tuesday { get; set; }

        [Display(Name = "Wed")]
        public bool Wednesday { get; set; }

        [Display(Name = "Thur")]
        public bool Thursday { get; set; }

        [Display(Name = "Fri")]
        public bool Friday { get; set; }

        [Display(Name = "Sat")]
        public bool Saturday { get; set; }
    }
}
