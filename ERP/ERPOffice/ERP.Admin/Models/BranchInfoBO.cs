using ERP.Address.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.Models
{
  public  class BranchInfoBO
    {
        public int BranchID { get; set; }
        //public int BranchCodeID { get; set; }
        [Required(ErrorMessage = "The BranchType field is required")]
        public int? BranchTypeID { get; set; }
        [Required(ErrorMessage ="The BranchStatus field is required")]
        public int ? BranchStatusID { get; set; }

        [Required,Display(Name ="Branch Name")]
        [System.Web.Mvc.Remote("BranchNameChk", "BranchInfo", "Admin", ErrorMessage = "Branch Name is Already exist...!!", AdditionalFields = "BranchID")]
        [MaxLength(50, ErrorMessage = "BranchName cannot be longer than 50 characters.")]
        public string BranchName { get; set; }

        [Required, Display(Name ="Branch Code")]
        [System.Web.Mvc.Remote("BranchCodeChk", "BranchInfo", "Admin", ErrorMessage = "Branch Code is Already exist...!!", AdditionalFields = "BranchID")]
        [MaxLength(50, ErrorMessage = "BranchCode cannot be longer than 50 characters.")]
        public string BranchCode { get; set; }

        [Display(Name ="Branch Type")]
        public string BranchType { get; set; }
        [Display(Name ="Branch Status")]
        public string BranchStatus { get; set; }
        public bool status { get; set; }

        
        [Display(Name = "Telephone No"),Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$", ErrorMessage = "Not a valid Phone number")]
        public string TelephoneNumber { get; set; }

        [Display(Name ="Fax No")]
        [MaxLength(50, ErrorMessage = "FAX No cannot be longer than 50 characters.")]
        public string FaxNumber { get; set; }
        [Required,Display(Name ="Vat No")]
        [MaxLength(20, ErrorMessage = "VAT No cannot be longer than 20 characters.")]
        public string VATnumber { get; set; }

    
        public long? AddressID { get; set; }

        //public AddressModel AddressModel { get;  set; }
        public AddressViewModel Address { get; set; }
    }
}
