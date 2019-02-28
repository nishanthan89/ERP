using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.Models
{
   public class CompanyInfoBO
    {
        public string HostKey { get; set; }
        public string HostValue { get; set; }

        public int AddressID { get; set; }
        public string CompanyName { get; set; }
        public string Currency { get; set; }
        public string EmailAddress { get; set; }
        public int FaxNo { get; set; }
        public string RegistrationNo { get; set; }
        public int TelephoneNo { get; set; }
        public string TimeZone { get; set; }
        public int VATNo { get; set; }
        public string Website { get; set; }
        public int DateFormatID { get; set; }
        public string DateFormat { get; set; }




        //[StringLength(100), Required(ErrorMessage = "Company Name is Required"), Display(Name = "Company Name")]
        //public string CompanyName { get; set; }
        //[Required, Display(Name = "VAT No")]
        //public string VatNo { get; set; }
        //[StringLength(50), Required(ErrorMessage = "General Email is Required"), Display(Name = "General Email"), DataType(DataType.EmailAddress)]
        //public string GeneralEmail { get; set; }
        //[Required, Display(Name = "Company License")]
        //public string CompanyLicense { get; set; }
        //[Required(ErrorMessage = "Time Zone is Required"), Display(Name = "Time Zone")]
        //public string TimeZoneID { get; set; }
        //[Display(Name = "Time Zone")]
        //public string TimeZone { get; set; }
        //[Required(ErrorMessage = "Date Format is Required"), Display(Name = "Date Format")]
        //public int DateFormatID { get; set; }
        //[Display(Name = "Date Format")]
        //public string DateFormat { get; set; }
        //[Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        //[StringLength(50), Display(Name = "Web Site")]
        //[Url(ErrorMessage = "Please enter a valid URL. Eg:http://www.bdl.lk")]
        //public string Website { get; set; }
        //[Required(ErrorMessage = "Currency is Required"), Display(Name = "Currency")]
        //public int CurrencyID { get; set; }
        //public string Currency { get; set; }
        //public byte[] Logo { get; set; }
        //public int AddressID { get; set; }

    }
}
