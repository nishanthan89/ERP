using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
    public class EmployeePaymentBO
    {
        public int? EmployeePayID { get; set; }

        [Display(Name = "Employee Name")]
        public int EmployeeID { get; set; }

        [Display(Name = "Amount")]
        [Required]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Price Must Be Greater Than 0.00")]
        public double? PaymentAmount { get; set; }

        [Display(Name = "Frequency")]
        [Required]
        public int PaymentTypeID { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        //[System.Web.Mvc.Remote("CheckEndDate", "EmployeePayment", "Resource", ErrorMessage = "End Date should be Greater than Start Date..", AdditionalFields = "EmployeePayID")]        
        public DateTime? EndDate { get; set; }

    }
}
