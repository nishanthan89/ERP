using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
	public class CompanyView
	{
		public CompanyBO Company { get; set; }
		public IEnumerable<CompanyBO> CompanyList { get; set; }
	}
}
