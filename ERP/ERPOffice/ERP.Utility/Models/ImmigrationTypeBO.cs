using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
   public  class ImmigrationTypeBO
    {
        public int ImmigrationTypeId { get; set; }
        [Required, Display(Name =" Immigration Status ")]
        public string ImmigrationStatus { get; set; }
    }
}
