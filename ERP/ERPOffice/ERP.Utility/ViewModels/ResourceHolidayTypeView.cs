using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
    public class ResourceHolidayTypeView
    {
        public ResourceHolidayTypeBO RHolidayTypeBO { get; set; }
        public IEnumerable<ResourceHolidayTypeBO> RHolidayTypeList { get; set; }
    }
}
