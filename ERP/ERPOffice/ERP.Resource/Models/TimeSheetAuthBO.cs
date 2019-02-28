using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
    public class TimeSheetAuthBO
    {
        public int? FrequencyID { get; set; }

        public int? ResourcesID { get; set; }
        [Display(Name = "Resource")]
        public string ResourcesName { get; set; }

        public int? StatusID { get; set; }
        [Display(Name = "Resource. Confm")]
        public string Status { get; set; }
        [Display(Name = "Confm. Date")]
        public DateTime? StatusDate { get; set; }
        [Display(Name = " Month Of Timesheet")]
        public DateTime? TimeSheetDate { get; set; }
        public int? TimesheetID { get; set; }
       
        [Display(Name = "No. Of Shift")]
        public int NoOFShift { get; set; }
        [Display(Name = "Total Shift Hours ")]
        public double? TolShiftHrs { get; set; }
        [Display(Name = "No. Of Holiday ")]
        public double? TolHly { get; set; }
        [Display(Name = "No. Of Holiday Hours")]
        public double? TolHlyHours { get; set; }
        [Display(Name = "Remarks ")]
        public string Comments { get; set; }
        [Display(Name = "Admin. Confm ")]
        public string locked { get; set; }
        [Display(Name = "Locked By")]
        public string LockedBy { get; set; }
        [Display(Name = "Locked Date Time")]
        public DateTime? LockedDateTime { get; set; }
        [Display(Name = " Time Sheet ")]
        public string TimeSheetFrequency { get; set; }
        [Display(Name = "HandOver Hours")]
        public double? HandOverHours { get; set; }
        [Display(Name = "Payment")]
        public double? Payment { get; set; }

    }
}
