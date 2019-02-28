using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
    public class ResourceShift
    {
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> ShiftPatternID { get; set; }
        public Nullable<System.DateTime> ShiftDate { get; set; }
        public Nullable<System.DateTime> ExpectedOnTime { get; set; }
        public Nullable<System.DateTime> ExpectedOffTime { get; set; }
        public Nullable<System.DateTime> ActualOnTime { get; set; }
        public Nullable<System.DateTime> ActualOffTime { get; set; }
        public Nullable<int> BranchID { get; set; }
    }
}
