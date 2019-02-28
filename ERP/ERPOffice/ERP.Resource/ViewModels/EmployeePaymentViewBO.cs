//--------------------------------------------------------------------------------
// <copyright file="EmployeePaymentViewBO.cs" company="BDL">
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Resource.Models;

namespace ERP.Resource.ViewModels
{
    public class EmployeePaymentViewBO
    {
        public IEnumerable<EmployeePaymentViewModels> EmployeePaymentList { get; set; } //Gets Payment List for a specific Employee
        public EmployeePaymentBO EmployeePaymentBO { get; set; }    //Employee Payment Models
    }
}
