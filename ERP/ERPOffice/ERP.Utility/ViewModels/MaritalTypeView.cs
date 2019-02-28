using ERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.ViewModels
{
    public class MaritalTypeView
    {
        public MaritalStatusBO MaritalStatus { get; set; }
        public IEnumerable<MaritalStatusBO> MaritalStatusList { get; set; }

    }
}
