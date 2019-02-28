using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
   public class EmployeeTypeView
    {
        public EmployeeTypeBO EmployeeType { get; set; }
        public IEnumerable<EmployeeTypeBO> EmployeeTypeList { get; set; }
    }
}
