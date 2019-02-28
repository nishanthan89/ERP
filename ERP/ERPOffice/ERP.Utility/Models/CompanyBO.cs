using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
	public class CompanyBO
	{
		public int CompanyID { get; set; }
		[Required, Display(Name ="Company Name")]
		public string CompanyName { get; set; }
	}
}
