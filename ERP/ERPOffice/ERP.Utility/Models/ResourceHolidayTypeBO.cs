using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
    public class ResourceHolidayTypeBO
    {
        public int HolidayTypeID { get; set; }
        [Display(Name = "Holiday Type Name"), Required]
		[System.Web.Mvc.Remote("CheckHolidayTypeExist", "MasterInfo", "Utility", ErrorMessage = "Holiday Type is already Exist!", AdditionalFields = "HolidayTypeID")]
		public string HolidayTypeName { get; set; }
        [Display(Name ="Abbreviated Holiday Type"), Required]
		[System.Web.Mvc.Remote("CheckAbbreviationExist", "MasterInfo", "Utility", ErrorMessage = "Abbreviation is already Exist!", AdditionalFields = "HolidayTypeID")]
		public string AbbriviatedHolidayType { get; set; }
        [Display(Name = "Color Code ")]
        public string IconCode { get; set; }
    }
}
