using ERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.ViewModels
{
   public  class EthnicTypeView
    {

        public EthnicTypeBO EthnicType  { get; set; }
        public IEnumerable<EthnicTypeBO> EthnicTypeList { get; set; }
    }
}
