//--------------------------------------------------------------------------------
// <copyright file="ResourceSubMenuBO.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

//  <Description>
//      It contains Models related to Employee Tabs
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

namespace ERP.Resource.ViewModels
{
    /// <summary>
    /// Class for Resource tabs in edit mode and It carries the EmployeeID, FirstName and LastName
    /// </summary>
    public class ResourceSubMenuBO
    {
        public string LName { get; set; }
        public string FName { get; set; }
        public int EmployeeID { get; set; }
    }
}
