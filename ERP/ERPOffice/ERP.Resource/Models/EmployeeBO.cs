using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.Models
{
    public class EmployeeBO
    {

        public int EmployeeID { get; set; }

        //[Display(Name = "Branch")]
        //[Required]
        public int BranchID { get; set; }

        //[Display(Name = "Title")]
        //[Required]
        public int? TitleID { get; set; }

        //[Display(Name = "First Name")]
        //[Required]
        public string FirstName { get; set; }

        //[Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        //[Display(Name = "Last Name")]
        //[Required]
        public string LastName { get; set; }

        //[Display(Name = "Date Of Birth")]
        public DateTime? DoB { get; set; }

        //[Display(Name = "Gender")]
        //[Required]
        public string Sex { get; set; }

        //[Display(Name = "Marital Status")]
        //[Required]
        public int? MaritalStatusID { get; set; }

        //[Display(Name = "Nationality")]
        //[Required]
        public int? NationalityID { get; set; }

        //[Display(Name = "Resource Type")]
        //[Required]
        public int? EmployeeTypeID { get; set; }

        //[Display(Name = "Ethnic Group")]
        //[Required]
        public int? EthnicGroupID { get; set; }

        //[Display(Name = "Date Joined"), Required]
        public DateTime? DateOfJoin { get; set; }

        //[Display(Name = "Date Left"), Required]
        public DateTime? DateOfLeave { get; set; }

        //[Display(Name = "Profile")]
        public byte[] ProfilePhoto { get; set; }

        public string PhotoFileType { get; set; }

        //[Display(Name = "NIC Number")]
        //[Required]
        //[RegularExpression(@"^\(?([0-9]{9})\)?[-. ]?([v]{1})$", ErrorMessage = "NIC Number is not valid, It should be 09 digits with v. Eg: 906573040v")]
        //[System.Web.Mvc.Remote("IsNICExist", "Employee", "Resource", ErrorMessage = "This NIC Number is already exist", AdditionalFields = "EmployeeID")]
        public string NINo { get; set; }

        //[Display(Name = "Immigration Status")]
        public int? ImmigrationStatusID { get; set; }

        //[Display(Name = "TimeSheet Frequency")]
        public int? TimeSheetFrequencyID { get; set; }

        //[Display(Name = "Address")]
        public int? AddressID { get; set; }

        //[Display(Name = "Email Address")]
        //[Required]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail Address")]
        public string Email { get; set; }

        //[Display(Name = "Home Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0211234567")]
        //[DataType(DataType.PhoneNumber)]
        public string TelephoneNo { get; set; }

        //[Display(Name = "Mobile Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0771234567")]
        //[DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        //[Display(Name = "Emergency Contact Name")]
        public string NextOfKIN_Name { get; set; }

        //[Display(Name = "Emergency Contact Home Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0111234567")]
        //[DataType(DataType.PhoneNumber)]
        public string KIN_TelephoneNo { get; set; }

        //[Display(Name = "Emergency Contact Mobile Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0771234567")]
        //[DataType(DataType.PhoneNumber)]
        public string KIN_MobileNo { get; set; }
    }
}
