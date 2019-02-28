//--------------------------------------------------------------------------------
// <copyright file="EmployeeViewBO.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

//  <Description>
//      It contains Models related to Employee
//  </Description>

// <Author>
//      T Genga 
// </Author>
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Address.ViewModels;
using ERP.Resource.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.Resource.ViewModels
{
    public class EmployeeViewBO : IValidatableObject
    {
        public int EmployeeID { get; set; }

        [Display(Name = "Branch")]
        //[Required]
        public int BranchID { get; set; }

        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [Display(Name = "Branch")]
        [Required]
        public List<int> BranchList { get; set; }    //Branch List for multi selection

        [Display(Name = "Branch Name")]
        public List<string> BList { get; set; }    //Branch Names for display the Branch List in the Resource "Index" View

        [Display(Name = "Title")]
        [Required]
        public int? TitleID { get; set; }
        public string TitleName { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "First Name Cannot Be Longer Than 50 Characters")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [MaxLength(50, ErrorMessage = "Middle Name Cannot Be Longer Than 50 Characters")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Last Name Cannot Be Longer Than 50 Characters")]
        public string LastName { get; set; }

        [Display(Name = "Date Of Birth")]
        [System.Web.Mvc.Remote("BirthDateCheck", "Employee", "Resource", ErrorMessage = "Birth Year Must Be Less Than 15 Years From This Year", AdditionalFields = "EmployeeID")]
        public DateTime? DoB { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public string Sex { get; set; }

        [Display(Name = "Marital Status")]
        [Required]
        public int? MaritalStatusID { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatusName { get; set; }

        [Display(Name = "Nationality")]
        [Required]
        public int? NationalityID { get; set; }
        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }

        [Display(Name = "Resource Type")]
        [Required]
        public int? EmployeeTypeID { get; set; }
        [Display(Name = "Resource Type")]
        public string EmployeeTypeName { get; set; }

        [Display(Name = "Ethnic Group")]
        [Required]
        public int? EthnicGroupID { get; set; }
        [Display(Name = "Ethnic Group")]
        public string EthnicGroupName { get; set; }

        [Display(Name = "Date Joined"), Required]
        public DateTime? DateOfJoin { get; set; }

        [Display(Name = "Date Left")]
        public DateTime? DateOfLeave { get; set; }

        [Display(Name = "Profile")]
        public byte[] ProfilePhoto { get; set; }
        public string PhotoFileType { get; set; }

        public string PhotoString { get; set; } //Dispaly purpose

        [Display(Name = "NIC Number")]
        [Required]
        //[RegularExpression(@"^\d{9}[V|v|x|X]$", ErrorMessage = "NIC Number is not valid, It should be 09 digits and ends with V Or X")]
        //[RegularExpression(@"^\(?([0-9]{9})\)?[-. ]?([V]{1})$", ErrorMessage = "NIC Number is not valid, It should be 09 digits ends with V. Eg: 906573040V")]
        [System.Web.Mvc.Remote("IsNICExist", "Employee", "Resource", ErrorMessage = "This NIC Number Is Already Exist", AdditionalFields = "EmployeeID")]
        public string NINo { get; set; }

        [Display(Name = "Home Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0211234567")]
        [RegularExpression(@"^\(?([+]{1})\)?[-. ]?([4]{2})[-. ]?([1-9]{2})[-. ]?([0-9]{8})$", ErrorMessage = "Please Enter A Valid Number. Eg: +447712345678")]
        [DataType(DataType.PhoneNumber)]
        public string TelephoneNo { get; set; }

        [Display(Name = "Mobile Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0771234567")]
        [RegularExpression(@"^\(?([+]{1})\)?[-. ]?([4]{2})[-. ]?([1-9]{2})[-. ]?([0-9]{8})$", ErrorMessage = "Please Enter A Valid Number. Eg: +447712345678")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        [Display(Name = "Immigration Status")]
        public int? ImmigrationStatusID { get; set; }
        public string ImmigrationStatusName { get; set; }

        [Display(Name = "Timesheet Frequency"), Required]
        public int? TimeSheetFrequencyID { get; set; }

        [Display(Name = "Timesheet Frequency")]
        public string TimeSheetFrequencyName { get; set; }

        [Display(Name = "Address")]
        public long? AddressID { get; set; }
        public string Town { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter A Valid E-Mail Address")]
        public string Email { get; set; }

        [Display(Name = "Emergency Contact Name")]
        public string NextOfKIN_Name { get; set; }

        [Display(Name = "Emergency Contact Home Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0111234567")]
        [RegularExpression(@"^\(?([+]{1})\)?[-. ]?([4]{2})[-. ]?([1-9]{2})[-. ]?([0-9]{8})$", ErrorMessage = "Please Enter A Valid Number. Eg: +447712345678")]
        [DataType(DataType.PhoneNumber)]
        public string KIN_TelephoneNo { get; set; }

        [Display(Name = "Emergency Contact Mobile Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0771234567")]
        [RegularExpression(@"^\(?([+]{1})\)?[-. ]?([4]{2})[-. ]?([1-9]{2})[-. ]?([0-9]{8})$", ErrorMessage = "Please Enter A Valid Number. Eg: +447712345678")]
        [DataType(DataType.PhoneNumber)]
        public string KIN_MobileNo { get; set; }

        public AddressViewModel AddressViewModel { get; set; }  //Address Models

        public IEnumerable<EmployeePaymentViewModels> EmployeePaymentList { get; set; } //Employee Payment List

        public EmployeePaymentBO EmployeePaymentBO { get; set; }    //Employee Payment Models

        public Employee_WorkingPatternDetailsBO WorkingPatternDetailsBO { get; set; }    //Employee Working Pattern Models

        public IEnumerable<Employee_WorkingPatternDetailsBO> EmployeeWorkingPatternList { get; set; } //Employee Working Pattern List

        public ResourceSubMenuBO ResourceSubMenuBO { get; set; }    //Models for Employee tabs in edit mode

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)  //"Validate" method which is inherits from "IValidatableObject" class
        {
            if (DateOfLeave != null && DateOfJoin != null && DateOfLeave < DateOfJoin)   //Model state validation for DateOfLeave, It should be greater than the DateOfJoin
            {
                yield return new ValidationResult(FirstName + "!... " + "Your 'Date Left' Must Be Greater Than The 'Date Joined'");    //Returns the error message
            }
            else if (DoB != null && DateOfJoin != null && DateOfJoin.Value.Year < DoB.Value.Year + 15)   //Model state validation for DateOfJoin, It should be greater than the DateOfBirth's year+15
            {
                yield return new ValidationResult(FirstName + "!... " + "Your 'Date Joined' Should Be Greater Than The Year Of Birth + 15 Years, Please Confirm Your 'Date Of Birth'");
            }
            //else if(BranchList.Count() == 0)
            //{
            //    yield return new ValidationResult(FirstName + "!... " + "Please Select At Least One Branch!");
            //}
            else if (EmployeeID == 0 && WorkingPatternDetailsBO.Sunday == false && WorkingPatternDetailsBO.Monday == false && WorkingPatternDetailsBO.Tuesday == false && WorkingPatternDetailsBO.Wednesday == false && WorkingPatternDetailsBO.Thursday == false && WorkingPatternDetailsBO.Friday == false && WorkingPatternDetailsBO.Saturday == false)   //Model state validation for DateOfLeave, It should be greater than the DateOfJoin
            {
                yield return new ValidationResult(FirstName + "!... " + "You Should Provide Atleast A Working Day For 'Resource Working Pattern'");    //Returns the error message
            }
            else if (EmployeeID == 0 && WorkingPatternDetailsBO.ChangeDate < DateOfJoin)   //Model state validation for ChangeDate, It should be greater than the DateOfJoin
            {
                yield return new ValidationResult(FirstName + "!... " + "Your Provided 'Change Date' Of 'Resource Working Pattern' Should Be Greater Than Or Equal To The Resource 'Date Joined' Field");    //Returns the error message
            }
            else if (AddressViewModel.CountryID != 1)   //When the selected Country is not "United Kingdom"
            {
                yield return new ValidationResult(FirstName + "!... " + "PAF Is Only Available For 'United Kingdom', So Please Select A Country As 'United Kingdom' !... ");    //Returns the error message
            }
        }
    }
}
