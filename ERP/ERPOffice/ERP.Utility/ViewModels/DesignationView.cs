using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;
namespace ERP.Utility.ViewModels
{
	public class DesignationView
	{
		public DesignationBO Designation { get; set; }
		public IEnumerable<DesignationBO> DesignationList { get; set; }
	}
}
