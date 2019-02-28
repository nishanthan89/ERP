using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
    public class NationalityBO
    {
        public int NationalityID { get; set; }
        [Required,Display(Name ="Nationality")]
        public string NationalityName { get; set; }
    }
}
