using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.ViewModels
{
   public class SearchHolidayViewModel
    {
        [Display(Name = "Resource")]
        public int? SearchResourceID { get; set; }
        public string Resource { get; set; }

        public int? Month { get; set; }
        public int? Year { get; set; }

        public int HolidayID { get; set; }
    }
}
