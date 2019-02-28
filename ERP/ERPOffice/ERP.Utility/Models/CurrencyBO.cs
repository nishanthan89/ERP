using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
   public class CurrencyBO
    {
        public int CurrencyID { get; set; }
        [Required,Display(Name ="Currency")]
        public string Currency { get; set; }
    }
}
