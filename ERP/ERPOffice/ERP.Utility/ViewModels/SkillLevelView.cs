using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
	public class SkillLevelView
	{
		public SkillLevelBO SkillLevel { get; set; }
		public IEnumerable<SkillLevelBO> SkillLevelList { get; set; }
	}
}
