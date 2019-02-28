using ERP.Resource.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERP.Resource.ViewModels
{
   public class ShiftScheduleViewModel
    {
        public int ShiftScheduleID { get; set; }
        [Required,Display(Name ="Employee")]
        public bool Resource { get; set; }
        //[Required(ErrorMessage = "The Employee field is required")]
        public List<WeeklyBO> ResourceNameList { get; set; }
        //[McvValidationAttribute(ErrorMessage = "At least an Employee is required")]
        [Display(Name = "Employee")]
        public List<Person> ResourceList { get; set; }
        [Required(ErrorMessage = "The Employee field is required. ")]
        [Display(Name = "Employee")]
        public int EditResourceID { get; set; }
        [Display(Name ="All")]
        public bool ResourceAll { get; set; }

        public int EmployeeID { get; set; }
        public string Employee { get; set; }

        [Required, Display(Name = "Branch Name")]
        public int BranchNameID { get; set; }
        public string BranchName { get; set; }
        public int BranchID { get; set; }


        [Required,Display(Name = "Shift Start Date")]
        public DateTime ShiftStartDate { get; set; }
        [Required, Display(Name = "Shift End Date")]
        public DateTime ShiftEndDate { get; set; }

        [Required, Display(Name = "Expected On Time")]
        public TimeSpan ExpectedOnTime { get; set; }
        [Required, Display(Name = "Expected Off Time")]
        public TimeSpan ExpectedOffTime { get; set; }

        public DateTime? ExpectedOnDateTime { get; set; }
        public DateTime? ExpectedOffDateTime { get; set; }

        [Display(Name = "Actual On Time")]
        public TimeSpan? ActualOnTime { get; set; }
        [Display(Name = "Actual Off Time")]
        public TimeSpan? ActualOffTime { get; set; }
       
        
        public DateTime? ActualOnDateTime { get; set; }
        public DateTime? ActualOffDateTime { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required, Display(Name = "Shift Pattern")]
        public int ShiftPatternID { get; set; }
        public string ShiftPattern  { get; set; }
        public int ShiftPatternNameID { get; set; }
        public float BasicRate { get; set; }
        public float OTRate { get; set; }
        public string YesNo { get; set; }

        [Required(ErrorMessage = "The Shift Schedule Activity field is required"),Display(Name = "Assign Type")]
        public string AssignType { get; set; }

        [Display(Name = "Mon")]
        public bool WeeklyMonday { get; set; }
        [Display(Name = "Tue")]
        public bool WeeklyTuesday { get; set; }

        [Display(Name = "Wed")]
        public bool WeeklyWednesday { get; set; }

        [Display(Name = "Thu")]
        public bool WeeklyThursday { get; set; }

        [Display(Name = "Fri")]
        public bool WeeklyFriday { get; set; }

        [Display(Name = "Sat")]
        public bool WeeklySaturday { get; set; }

        [Display(Name = "Sun")]
        public bool WeeklySunday { get; set; }

        [Display(Name = "No of Days")]
        public int NoofDays { get; set; }
    }
    public class Person
    {
        public string Resource { get; set; }
        public int ResourceID { get; set; }
        public bool ResourceCheck { get; set; }
    }
}
