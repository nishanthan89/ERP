using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
    public class PaymentTypeBO
    {
        public int PaymentID { get; set; }
        [Required, Display(Name ="Payment Frequency")]
        public string PaymentName { get; set; }
    }
}
