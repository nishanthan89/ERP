using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
    public class CurrencyView
    {
        public CurrencyBO  Currency{ get; set; }
        public IEnumerable<CurrencyBO> currencyList { get; set; }
    }
}
