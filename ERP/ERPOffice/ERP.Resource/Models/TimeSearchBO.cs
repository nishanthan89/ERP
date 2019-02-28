using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
    public class TimeSearchBO
    {

        public int? ResourcesID { get; set; }
        [Display(Name = "Resource")]
        public string ResourcesName { get; set; }
        public int MonthID { get; set; }
        [Display(Name = "Month ")]
        public string MonthName { get; set; }
        public int? YearID { get; set; }
        [Display(Name = "Year")]
        public string Year { get; set; }
        public int? StatusID { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }

        public int? FrequencyID { get; set; }
        [Display(Name = "Frequency")]
        public string Frequency { get; set; }


    }
}
