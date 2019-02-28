using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.ViewModels
{
	public class EmployeeView
	{
		[Display(Name ="Employee ID")]
		public int? EmployeeID { get; set; }
		[Display(Name = "Name")]
		public string FName { get; set; }
		public string LName { get; set; }
		
	}
}
