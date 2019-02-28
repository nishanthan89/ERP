using ERP.Address.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.ViewModels
{
    public class CompanyInfoViewModel
    {

        //public string HostKey { get; set; }
        //public string HostValue { get; set; }
        [Display(Name = " Address ")]
        public long? AddressID { get; set; }
       [Required(ErrorMessage = "Company Name is Required."), Display(Name = "Company Name")]
       [StringLength(50, ErrorMessage = "Company Name cannot be longer than 50 characters.")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Currency is Required."), Display(Name = "Currency")]
        public int CurrencyID { get; set; }
        public string Currency { get; set; }
        [StringLength(50), Required(ErrorMessage = "General Email is Required."), Display(Name = "General Email"), DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Fax No is Required."), Display(Name = "Fax No")]
        [MaxLength(50, ErrorMessage = "FAX No cannot be longer than 50 characters.")]
        public string FaxNo { get; set; }
        [Required(ErrorMessage = "Registration No is Required."), Display(Name = "Registration No")]
        [MaxLength(50, ErrorMessage = "Registration No cannot be longer than 50 characters.")]
        public string RegistrationNo { get; set; }
        [Required(ErrorMessage = "Telephone No is Required.")]
        [Display(Name = "Telephone No")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number.")]
       
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$", ErrorMessage = "Not a valid Phone number")]
        public long TelephoneNo { get; set; }
        [Required(ErrorMessage = "Time Zone is Required."), Display(Name = "Time Zone")]
        public string TimeZoneID { get; set; }
        [Display(Name = "Time Zone")]
        public string TimeZone { get; set; }
        [Required(ErrorMessage = "VAT No is Required."), Display(Name = "Vat No")]
        [MaxLength(50, ErrorMessage = "VAT No cannot be longer than 50 characters.")]
        public string VATNo { get; set; }
        //[Required, Display(Name = "License No")]
        //public string LicienceNO { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [StringLength(50), Display(Name = "Website") ]
        [Url(ErrorMessage = "Please enter a valid URL.Eg:http://www.bdl.lk ")]
        public string Website { get; set; }
        [Required(ErrorMessage = "Date Format is Required."), Display(Name = "Date Format")]
        public int DateFormatID { get; set; }
        [Display(Name = "Date Format")]
        public string DateFormat { get; set; }
        public int LogoID { get; set; }
        [Required(ErrorMessage = "Time Format is Required."), Display(Name = "Time Format")]
        public int TimeID { get; set; }
        [Display(Name = "Time Format")]
        public string TimeFormat { get; set; }

        [Display(Name = "Profile")]
        public byte[] ProfilePhoto { get; set; }
        public string PhotoFileType { get; set; }

        public string PhotoString { get; set; } //Dispaly purpose


        public AddressViewModel Address { get; set; }
    }
    public class DateFormatModel
    {
        public int DateFormatID { get; set; }
        public string DateFormat { get; set; }
    }

    public  class CurrencyFormat
    {
        public int CurrencyID { get; set; }
        public string Currency { get; set; }
    }

   
}
