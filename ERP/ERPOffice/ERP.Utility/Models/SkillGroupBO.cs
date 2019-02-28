using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
	public class SkillGroupBO
	{
		public int SkillGroupID { get; set; }
		public int? SkillID { get; set; }
		public string SkillName { get; set; }
		public int? SkillCategoryID { get; set; }
		public int? SkillLevelID { get; set; }
		public string SkillLevel { get; set; }
	}
}
