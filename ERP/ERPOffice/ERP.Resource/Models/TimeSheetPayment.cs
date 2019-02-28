using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{

    public class TimeSheetPayment
    {
        public Tuple<DateTime, DateTime> dateRange { get; set; }
        public double? Payment { get; set; }
        public int Pfrequency { get; set; }
        public DateTime? PstartDate { get; set; }
        public DateTime? PendDate { get; set; }
        public int PEmpID { get; set; }
        public string PEmpName { get; set; }
    }
}
