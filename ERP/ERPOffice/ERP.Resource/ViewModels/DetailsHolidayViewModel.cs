using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.ViewModels
{
     public  class DetailsHolidayViewModel
    {
        [Display(Name = "Status")]
        public int? StatusID { get; set; }
        public string Status { get; set; }
        [Display(Name = "Status Changed By")]
        public int? StatusChangedByID { get; set; }
        public string StatusChangedBy { get; set; }

        [Display(Name = "Status Changed On")]
        public DateTime? StatusChangedOn { get; set; }

        public int HolidayID { get; set; }
    }
}
