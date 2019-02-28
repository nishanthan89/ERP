using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
    public class TSFrequencyBO
    {        
        public int TimeSheetFrequencyID { get; set; }
        [Display(Name = "Timesheet Frequency Name "), Required]
        public string TimeSheetFrequencyName { get; set; }
        [Display(Name = "Timesheet Frequency Deadline"), Required]
        public string TimeSheetFrequencyDeadline { get; set; }
        //public string weeklyDeadLine { get; set; }

    }
}
