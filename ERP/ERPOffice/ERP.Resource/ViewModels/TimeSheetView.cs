using ERP.Resource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.ViewModels
{
    public class TimeSheetView
    {
        public TimeSearchBO TsearchBBO { get; set; }
        public TimeSheetBO timeSheetBBO { get; set; }
        public IEnumerable<TimeSheetBO> timeSheetView { get; set; }
        public int? graphID { get; set; }
        
    }
}
