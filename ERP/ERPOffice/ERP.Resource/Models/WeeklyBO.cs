using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
    public class WeeklyBO
    {
        public string ResourceWeekly { get; set; }
        public List<DateTime> ShiftStartDateList { get; set; }
        public List<string> ShiftPatternList { get; set; }
    }
}
