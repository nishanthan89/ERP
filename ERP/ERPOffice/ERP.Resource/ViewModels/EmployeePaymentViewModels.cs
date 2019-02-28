//--------------------------------------------------------------------------------
// <copyright file="EmployeePaymentViewModels.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

//  <Description>
//      It contains Models related to Employee Payment
//  </Description>

// <Author>
//      T Genga 
// </Author>
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.ViewModels
{
    public class EmployeePaymentViewModels
    {
        public int EmployeePayID { get; set; }

        //[Required]
        public int EmployeeID { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Amount")]
        [Required]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Price Must Be Greater Than 0.00")]
        public double? PaymentAmount { get; set; }

        [Required]
        public int PaymentTypeID { get; set; }

        [Display(Name = "Frequency")]
        public string PaymentTypeName { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public List<DateTime?> TimesheetAuthorizedDateList { get; set; }    //Timesheet Authorized Date List for a particular Employee
    }
}
