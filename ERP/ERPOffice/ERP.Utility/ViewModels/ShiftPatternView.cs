using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
	public class ShiftPatternView
	{
		public ShiftPatternBO ShiftPattern { get; set; }
		public IEnumerable<ShiftPatternBO> ShiftPatternList { get; set; }
	}
}
