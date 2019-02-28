using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Address.ViewModels
{
   public  class AddressViewModel
    {
        public long? AddressID { get; set; }
        [Display(Name = "Building Name")]
        public string BuildingName { get; set; }
        [Required]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }
        [Display(Name = "Locality")]
        public string Locality { get; set; }
        [Required]
        [Display(Name = "Town")]
        public string Town { get; set; }
        [Required]
        [Display(Name = "County")]
        public string County { get; set; }
        [Required]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }
        [Required]
        [Display(Name = "Country")]
        public int CountryID { get; set; }

        public string Country { get; set; }


       
    }

   
}
