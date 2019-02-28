using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility.Models
{
    public class CountryBO
    {
        public int CountryID { get; set; }
        [Required, Display(Name = "Country Name")]
		//remote validation to check the country name already exist, CountryId id is required when update.
		[System.Web.Mvc.Remote("CheckCountryNameExist", "MasterInfo", "Utility", ErrorMessage = "Country Name is already Exist!", AdditionalFields = "CountryID")]
		public string CountryName { get; set; }
		[System.Web.Mvc.Remote("CheckCountryCodeExist", "MasterInfo", "Utility", ErrorMessage = "Country Code is already Exist!", AdditionalFields = "CountryID")]//remote validation to check 
		[Required, Display(Name = "Country Code")]
        public string CountryCode { get; set; }
        [Required, Display(Name = "Is Selected")]
		public bool IsSelected { get; set; }
    }
}
