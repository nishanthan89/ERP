using ERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.ViewModels
{
    public class NationalityView
    {
        public NationalityBO Nationality { get; set; }
        public IEnumerable<NationalityBO> NationalityList { get; set; }
    }
}
