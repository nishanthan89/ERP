using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
    public class CountryView
    {
        public CountryBO Country { get; set; }
        public IEnumerable<CountryBO> CountryList { get; set; }
    }
}
