using ERP.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.ViewModels
{
   public  class BranchInfoViewModel
    {
        public BranchInfoBO branchInfoBO { get; set; }
        public IEnumerable<BranchInfoBO> branchInfoViewModel { get; set; }


    }
}
