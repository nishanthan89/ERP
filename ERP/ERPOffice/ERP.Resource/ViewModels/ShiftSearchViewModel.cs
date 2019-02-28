using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.ViewModels
{
   public class ShiftSearchViewModel
    {
        [Display(Name ="Employee")]
        public int? EmployeeID { get; set; }
        public string Employee { get; set; }

        [Display(Name ="Branch")]
        public int? BranchID { get; set; }

        [Required, Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public int ShiftScheduleID { get; set; }
        
        public int BlockNumber { get; set; }

        [Display(Name ="Shift Pattern")]
        public int? ShiftPatternNameID { get; set; }

        [Display(Name = "Select View")]
        public string SelectView { get; set; }

        public int CountData { get; set; }
    }
    
}
