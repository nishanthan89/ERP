using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
	public class SkillCategoryBO
	{
		public int SkillCategoryID { get; set; }
		[Required, Display(Name = "Category Name")]
		public string CategoryName { get; set; }
		public string Description { get; set; }
	}
}
