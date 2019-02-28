using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
	public class SkillBO
	{
		[ScaffoldColumn(false)]
		public int? SkillID { get; set; }
		[Required, Display(Name = "Skill Name")]
		public string SkillName { get; set; }
		[Display(Name = "Skill Description")]
		public string SkillDescription { get; set; }
		public int NoOfResources { get; set; }

	}
}
