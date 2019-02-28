using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
   public class TSFrequencyView
    {
        public TSFrequencyBO Frequency { get; set; }
        public IEnumerable<TSFrequencyBO> TSFrequencyList { get; set; }
        //public string weeklyDeadLine { get; set; }
    }
}
