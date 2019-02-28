using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
    public class MaritalStatusBO
    {
        public int MaritalId { get; set; }
        [Required, Display(Name = "Marital Status ")]
        public string MaritalStatus{ get; set; }
    }
}
