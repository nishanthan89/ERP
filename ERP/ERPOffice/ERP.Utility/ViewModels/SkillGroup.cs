using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility.Models;

namespace ERP.Utility.ViewModels
{
	public class SkillGroup
	{
		//public List<SkillCategoryBO> SkillCategoryList { get; set; }
		public int SkillGroupID { get; set; }
		[Display(Name = "Category Name")]
		public string CategoryName { get; set; }
		public int CategoryID { get; set; }
		public string CategoryDescription { get; set; }
		public bool isSelected { get; set; }
		public int? SkillLevelID { get; set; }
		public string SkillLevelName { get; set; }
		public string SKillLevelDescription { get; set; }

		public SkillCategoryBO CategoryBO { get; set; }
		public SkillLevelBO LevelBO { get; set; }
	}
}
