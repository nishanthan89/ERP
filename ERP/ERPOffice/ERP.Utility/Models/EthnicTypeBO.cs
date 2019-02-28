using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
   public class EthnicTypeBO
    {
        public int EthGrpID { get; set; }
        [Required, Display(Name = "Resource Ethnicity")]
        public string EthGrpName { get; set; }
    }
}
