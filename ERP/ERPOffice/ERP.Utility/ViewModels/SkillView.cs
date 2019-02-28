using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
	public class SkillView
	{
		//[ScaffoldColumn(false)]
		//public int SkillID { get; set; }

		//[Required, Display(Name ="Skill Name")]
		//public String SkillName { get; set; }

		//[Display(Name = "Skill Description")]
		//public string SkillDescription { get; set; }

		public SkillBO Skill { get; set; }

		public List<SkillGroup> SkillCategoryGroup { get; set; }
		public IEnumerable<SkillBO> SkillList { get; set; }

										   //public int SkillCategoryID { get; set; }
										   //public int SkillLevelID { get; set; }

		//public List<SkillCategoryBO> SkillCategoryList { get; set; }

		//public IEnumerable<SkillLevelBO> SkillLevelList { get; set; }
	}
}
