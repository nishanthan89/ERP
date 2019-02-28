using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.ViewModels
{
   public class CreateHolidayViewModel
    {
        [Required,Display(Name ="Resource")]
        public int  ResourceID { get; set; }
        public string Resource { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
        public int? SearchResourceID { get; set; }

        public int HolidayID { get; set; }
        [Required,Display(Name ="Holiday Type")]
        public int HolidayTypeID { get; set; }
        public string HolidayType { get; set; }
        [Display(Name ="Holiday Date")]
        public DateTime HolidayDate { get; set; }
        [Required(ErrorMessage = "The From field is required."),Display(Name ="From:")]
        public DateTime HolidayStartDate { get; set; }
        [Required(ErrorMessage = "The To field is required."),Display(Name ="To:")]
        public DateTime HolidayEndDate { get; set; }
        [Display(Name ="Booking Hours")]
        public float BookingHrs { get; set; }
        public string Comment { get; set; }
        public string HoliColor { get; set; }

        [Required,Display(Name = "Status")]
        public int StatusID { get; set; }
        public string Status { get; set; }
        [Display(Name = "Status Changed By")]
        public int StatusChangedByID { get; set; }
        [Display(Name = "Status Changed By")]
        public string StatusChangedBy { get; set; }

        [Display(Name = "Requested On")]
        public DateTime RequestedOn { get; set; }
        [Display(Name = "Status Changed On")]
        public DateTime? StatusChangedOn { get; set; }

        [Required(ErrorMessage = "The Holiday Assign Type field is required."), Display(Name = "Assign Type")]
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

        public SearchHolidayViewModel SearchHoliday { get; set; }
    }
}
