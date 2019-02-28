using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
    public class AuthorizedTimeSheet
    {
        public int TimeSheetID { get; set; }
        public int ResourcesID { get; set; }
        public DateTime? TimeSheetDate { get; set; }
        public int TimeSheetFerquencyId { get; set; }
        public DateTime? TimeSheetAuthorizedDate { get; set; }
        public string Approved{ get; set; }
        public string LockedBy { get; set; }
        public DateTime? LockedDateTime { get; set; }
        public string Comments { get; set; }

    }
}
