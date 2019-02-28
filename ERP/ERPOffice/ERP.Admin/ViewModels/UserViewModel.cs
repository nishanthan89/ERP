using ERP.Address.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        public string UserID { get; set; }
        [Required(ErrorMessage = "The Employee No field is required."), Display(Name = "Employee No")]
        [System.Web.Mvc.Remote("IsEmpExist", "Register", "Admin", ErrorMessage = "Employee No already exist", AdditionalFields = "UserID")]
        public string EmployeeNO { get; set; }

        [Required(ErrorMessage = "The Title field is required"),Display(Name ="Title")]
        public int TitleID { get; set; }
        public string Title { get; set; }
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Username")]
        [System.Web.Mvc.Remote("IsUserExist", "Register","Admin", ErrorMessage = "User name already exist",AdditionalFields ="UserID")]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0771234567")]
        public string Mobile { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter 10 digits Number. Eg: 0771234567")]
        public string Telephone { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime DOB { get; set; }
        [Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }
        public int AddressID { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "The JobTitle field is required."),Display(Name ="Job Title")]
        public int JobTitleID { get; set; }
        public string JobTitle { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Status Comment")]
        public string StatusComment { get; set; }

        public string LoginUserID { get; set; }
        public DateTime LoginDate { get; set; }

        [Required]
        public int Day { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }

        public AddressViewModel AddressViewModel { get; set; }
    }
}
