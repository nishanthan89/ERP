using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
	public class ShiftPatternBO
	{
		public int ShiftPatternID { get; set; }

		[Display(Name = "Shift Pattern"), Required]
		//[RegularExpression(@"^\(?([0-9]{2})\)?[:. ]?([0-9]{2})[- ]?([0-9]{2})[:. ]?([0-9]{2})$", ErrorMessage = "Shift Pattern Should be a format. Example 08:00-10:30")]
		[System.Web.Mvc.Remote("CheckShiftPattern", "MasterInfo", "Utility", ErrorMessage = "Shift Pattern already Exist!", AdditionalFields = "ShiftPatternID")]
		public string Description { get; set; }

		[DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
		[Required, Display(Name = "Shift Start Time")]
		public TimeSpan ShiftStartTime { get; set; }

		[DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
		[Required, Display(Name = "Shift Duration")]
		//[DisplayFormat(DataFormatString = "{0:HH-mm}", ApplyFormatInEditMode = true)]
		[Range(typeof(TimeSpan), "02:00", "11:00")]
		public TimeSpan Duration { get; set; }

		[Display(Name = "Enabled")]
		public bool IsEnable { get; set; }

		[Display(Name = "Break Duration In Min")]
		[Range(typeof(int),"0","60")]
		public int? BreakDuration { get; set; }

		public int ResourceCount { get; set; }
	}
}
