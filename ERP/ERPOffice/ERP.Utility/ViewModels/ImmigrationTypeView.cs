using ERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.ViewModels
{
   public class ImmigrationTypeView
    {
        public ImmigrationTypeBO ImmigrationType  { get; set; }
        public IEnumerable<ImmigrationTypeBO> ImmigrationTypeList { get; set; }
    }
}
