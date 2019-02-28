using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{

   public class TimeSheetHolidayPattern
    {
        public DateTime? HolidayDate { get; set; }
        public string HolidayType { get; set; }
        public string Comment { get; set; }
        public string Authorized { get; set; }
        public double? HolidayHours { get; set; }
    }
}
