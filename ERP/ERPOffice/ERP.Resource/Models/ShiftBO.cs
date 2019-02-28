using ERP.Resource.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
   public class ShiftBO
    {
        public ShiftSearchViewModel ShiftSearch { get; set; }
        public ShiftScheduleViewModel ShiftSchedule { get; set; }

        public List<ShiftScheduleViewModel> ShiftScheduleList { get; set; }
        
    }
    public class JsonModel
    {
        public string HTMLString { get; set; }
        public bool NoMoreData { get; set; }
        public int DataCounts { get; set; }
    }
    public class ShiftOverride
    {
        public List<string> ListName { get; set; }
        public string ShiftStartDate { get; set; }
    }
}
