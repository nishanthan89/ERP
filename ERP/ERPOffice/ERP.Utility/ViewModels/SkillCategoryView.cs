using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
	public class SkillCategoryView
	{
		public SkillCategoryBO SkillCategory { get; set; }
		public IEnumerable<SkillCategoryBO> SkillCategoryList { get; set; }
	}
}
