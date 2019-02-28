//--------------------------------------------------------------------------------
// <copyright file="EmployeeSearchBO.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

//  <Description>
//      It contains Models related to Employee Search
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

using System.ComponentModel.DataAnnotations;
namespace ERP.Resource.ViewModels
{
    public class EmployeeSearchBO
    {
        //public EmployeeViewBO employeeViewBO { get; set; }
        public string SearchBy { get; set; }

        [Display(Name = "Search: ")]
        public string Text { get; set; }

        public int? BranchID { get; set; }
        public string BranchName { get; set; }
        public IEnumerable<EmployeeViewBO> EmployeeList { get; set; }   //Gets Employee List
    }
}
