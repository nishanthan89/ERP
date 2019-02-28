using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERP.Utility.Models
{
    public class TitleBO
    {     
        public int TitleID { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
