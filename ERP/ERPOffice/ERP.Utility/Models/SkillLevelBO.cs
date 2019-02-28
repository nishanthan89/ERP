using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
	public class SkillLevelBO
	{
		[ScaffoldColumn(false)]
		public int SkillLevelID { get; set; }
		[Required, Display(Name = "Skill Level Name")]
		public string SkillLevelName { get; set; }
		public string Description { get; set; }
		[Display(Name = "Max Skill Level")]
		[Range(typeof(int), "0", "10")]
		public int? MaxSkillLevel { get; set; }
		[Display(Name = "Min Skill Level")]
		[Range(typeof(int), "0", "10")]
		public int? MinSkillLevel { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)  //"Validate" method which is inherits from "IValidatableObject" class
		{
			if (MinSkillLevel > MaxSkillLevel)   //Model state validation for EndYear, It should be greater than the StartYear
			{
				yield return new ValidationResult("'Max Skill Level' must be greater than 'Min SKill Level'");    //Returns the error message
			}			
		}
	}
}
