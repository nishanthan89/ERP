using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
    public class EmployeeTypeBO
    {
        public int EmployeeTypeID { get; set; }
        [Display(Name = "Resource Type"),Required]
        public string EmployeeTypeName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Reporting To")]
        public int? ReportingTo { get; set; }
        public string ReportingEmployee { get; set; }
    }
}
