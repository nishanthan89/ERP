using ERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.ViewModels
{
    public class TitleView
    {
      
        public TitleBO Title { get; set; }
        public IEnumerable<TitleBO> TitleList { get; set; }
    }
}
