//--------------------------------------------------------------------------------
// <copyright file="EmployeeWorkingPatternDetailsViewBO.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

//  <Description>
//      It contains Models related to Employee Working Pattern
//  </Description>

// <Author>
//      T Genga 
// </Author>
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Resource.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.Resource.ViewModels
{
    public class EmployeeWorkingPatternDetailsViewBO
    {
        public IEnumerable<Employee_WorkingPatternDetailsBO> EmployeeWorkingPatternList { get; set; } //Gets WorkingPattern List for a specific Employee
        public Employee_WorkingPatternDetailsBO Employee_WorkingPatternDetailsBO { get; set; }    //Employee WorkingPattern Models
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
    }
}
