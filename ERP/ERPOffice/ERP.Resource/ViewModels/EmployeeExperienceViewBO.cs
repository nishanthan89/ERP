//--------------------------------------------------------------------------------
// <copyright file="EmployeeExperienceViewBO.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

//  <Description>
//      It contains Models related to Employee Experience
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
using ERP.Resource.Models;

namespace ERP.Resource.ViewModels
{
    public class EmployeeExperienceViewBO
    {
        public int EmployeeID { get; set; }
        public ResourceSubMenuBO ResourceSubMenuBO { get; set; }    //Models for Employee tabs in edit mode
        public EmployeeExperienceViewModels EmployeeExperienceViewModels { get; set; }
        public IEnumerable<EmployeeExperienceViewModels> EmployeeExpereienceList { get; set; } //Gets Expereience List for a particular Employee
    }
}
