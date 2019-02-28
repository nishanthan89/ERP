using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
  
    public class TimeSheetShiftPattern
    {
        public int? ShiftStartTime { get; set; }
        public int? Duration { get; set; }
        public int ShiftpatternId { get; set; }
        public string Description { get; set; }
        public int Breakduration { get; set; }
        public DateTime? ShiftDate { get; set; }
    }
}
