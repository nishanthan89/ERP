using ERP.Address.BL;
using ERP.Address.ViewModels;
using ERP.Admin.ViewModels;
using ERP.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Admin.BL
{
    public class RegisterBL
    {
        private ERPEntities db = new ERPEntities();
        private AddressBL addressBL = new AddressBL();      //Gets all the methods from "AddressBL"
        /// <summary>
        /// Save the User Details
        /// </summary>
        /// <param name="userViewModel"></param>
        public bool CreateUser(UserViewModel userViewModel,out string msg)
        {
            try
            {
                if (userViewModel.AddressViewModel.Postcode != null)
                {
                    userViewModel.AddressID = (int)addressBL.CheckAddress(userViewModel.AddressViewModel);    //Assign a AddressID for Employee
                    userViewModel.AddressViewModel.AddressID = userViewModel.AddressID;
                    if (userViewModel.AddressViewModel.AddressID == 0)
                    {
                        msg = "AddressID Is Not Saved!.....";
                        return false;
                    }
                }
            userViewModel.DOB = new DateTime(userViewModel.Year, userViewModel.Month, userViewModel.Day);
            User_UserDetails users = new User_UserDetails();
            users.UserId = userViewModel.UserID;
            users.TitleID = (int)userViewModel.TitleID;
            users.FirstName = userViewModel.FirstName;
            users.LastName = userViewModel.LastName;
            users.UserName = userViewModel.Username;
            users.Telephone = userViewModel.Telephone;
            users.Mobile = userViewModel.Mobile;
            users.DateofBirth = userViewModel.DOB;
            users.EmployeeNo = userViewModel.EmployeeNO;
            users.JoinDate = userViewModel.JoinDate;
            users.Email = userViewModel.Email;
            users.Status = userViewModel.Status;
            users.StatusComment = userViewModel.StatusComment;
            users.AddressID = userViewModel.AddressID;
            users.JobTitleID = (short)userViewModel.JobTitleID;
            db.User_UserDetails.Add(users);
            db.SaveChanges();
            msg = "Successfully Created!";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// GET the User Details 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserViewModel> GetUserDetail()
        {
            IEnumerable<UserViewModel> usrdetails = (from ausr in db.AspNetUsers
                                                     join uusr in db.User_UserDetails on ausr.Id equals uusr.UserId
                                                     select new UserViewModel
                                                     {
                                                         FirstName = uusr.FirstName,
                                                         LastName = uusr.LastName,
                                                         Username = uusr.UserName,
                                                         AddressID = uusr.AddressID,
                                                         Email = uusr.Email,
                                                         Telephone = uusr.Telephone,
                                                         Mobile = uusr.Mobile,
                                                         EmployeeNO = uusr.EmployeeNo,
                                                         Title = uusr.Common_Title.TitleName,
                                                         UserID = uusr.UserId,
                                                         JobTitle = uusr.Common_JobTitle.JobTitleName,
                                                         LoginUserID = db.AspNetLoginOffs.Where(x => x.UserID == uusr.UserId).Select(x => x.UserID).FirstOrDefault(),
                                                         LoginDate = db.AspNetLoginOffs.Where(x => x.UserID == uusr.UserId).Select(x => x.Login).FirstOrDefault()
                                                     }).Where(x=>x.Username!="sysadmin");
            return usrdetails.ToList();
        }
        /// <summary>
        /// GET the User Details Search
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserViewModel> GetUserDetailSearch(string searchBy, string searchText)
        {
            IEnumerable<UserViewModel> usrdetails = (from ausr in db.AspNetUsers
                                                     join uusr in db.User_UserDetails on ausr.Id equals uusr.UserId
                                                     select new UserViewModel
                                                     {
                                                         FirstName = uusr.FirstName,
                                                         LastName = uusr.LastName,
                                                         Username = uusr.UserName,
                                                         AddressID = uusr.AddressID,
                                                         Email = uusr.Email,
                                                         Telephone = uusr.Telephone == null ? String.Empty : uusr.Telephone,
                                                         Mobile = uusr.Mobile,
                                                         EmployeeNO = uusr.EmployeeNo,
                                                         Title = uusr.Common_Title.TitleName,
                                                         UserID = uusr.UserId,
                                                         JobTitle = uusr.Common_JobTitle.JobTitleName,
                                                         LoginUserID = db.AspNetLoginOffs.Where(x => x.UserID == uusr.UserId).Select(x => x.UserID).FirstOrDefault(),
                                                         LoginDate = db.AspNetLoginOffs.Where(x => x.UserID == uusr.UserId).Select(x => x.Login).FirstOrDefault()
                                                     }).Where(x => x.Username != "sysadmin");
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                // SearchText = SearchText.ToLower();
                if (searchBy == "EmployeeNo")
                {
                    usrdetails = usrdetails.Where(u => u.EmployeeNO.ToLower().Contains(searchText.ToLower()));
                }
                else if (searchBy == "FirstName")
                {
                    usrdetails = usrdetails.Where(u => u.FirstName.ToLower().Contains(searchText.ToLower()));
                }
                else if (searchBy == "LastName")
                {
                    usrdetails = usrdetails.Where(u => u.LastName.ToLower().Contains(searchText.ToLower()));
                }
                else if (searchBy == "UserName")
                {
                    usrdetails = usrdetails.Where(u => u.Username.ToLower().Contains(searchText.ToLower()));
                }
                else if (searchBy == "Address")
                {
                    usrdetails = usrdetails.Where(u => u.Address.ToLower().Contains(searchText.ToLower()));
                }
                else if (searchBy == "Telephone")
                {
                    usrdetails = usrdetails.Where(u => u.Telephone.ToLower().Contains(searchText.ToLower()));
                }
                else if (searchBy == "Mobile")
                {
                    usrdetails = usrdetails.Where(u => u.Mobile.ToLower().Contains(searchText.ToLower()));
                }
                else if (searchBy == "Email")
                {
                    usrdetails = usrdetails.Where(u => u.Email.ToLower().Contains(searchText.ToLower()));
                }
            }
            return usrdetails.Select(x => x.Username).Count() != 0 ? usrdetails.ToList() : null;
        }
        /// <summary>
        /// DeleteUser
        /// </summary>
        /// <param name="id"></param>
        public bool DeleteUserDetails(string id,out string msg)
        {
            msg = "";
            try
            {
            AspNetUser aspNetUser = (from ausr in db.AspNetUsers where ausr.Id == id select ausr).FirstOrDefault();
            User_UserDetails userUserDetails = (from uusr in db.User_UserDetails where uusr.UserId == id select uusr).FirstOrDefault();
            if (aspNetUser != null)
            {
                db.AspNetUsers.Remove(aspNetUser);
            }
            if (userUserDetails != null)
            {
                db.User_UserDetails.Remove(userUserDetails);
            }
            db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

            return true;
        }
        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserViewModel EditUserDetails(string id,out string msg)
        {
            msg = "";
            try
            {
            var eusr = (from ausr in db.AspNetUsers
                        join uusr in db.User_UserDetails on ausr.Id equals uusr.UserId
                        where ausr.Id == id
                        select new UserViewModel
                        {
                            Email = ausr.Email,
                            FirstName = uusr.FirstName,
                            LastName = uusr.LastName,
                            AddressID = uusr.AddressID,
                            Telephone = uusr.Telephone,
                            TitleID = uusr.TitleID,
                            Username = uusr.UserName,
                            EmployeeNO = uusr.EmployeeNo,
                            Status = uusr.Status,
                            StatusComment = uusr.StatusComment,
                            Mobile = uusr.Mobile,
                            DOB = uusr.DateofBirth,
                            JoinDate = uusr.JoinDate,
                            UserID = uusr.UserId,
                            JobTitleID = uusr.JobTitleID,
                            Year = uusr.DateofBirth.Year,
                            Month = uusr.DateofBirth.Month,
                            Day = uusr.DateofBirth.Day,
                            AddressViewModel=new AddressViewModel
                            {
                                AddressID = uusr.AddressID,
                                BuildingName = uusr.Common_Address.AddressLine1,
                                StreetName = uusr.Common_Address.AddressLine2,
                                Locality = uusr.Common_Address.AddressLine3,
                                Town = uusr.Common_Address.Town,
                                County = uusr.Common_Address.County,
                                Postcode = uusr.Common_Address.PostCode,
                                CountryID = uusr.Common_Address.CountryID.Value
                            }
                        }).FirstOrDefault();

            return eusr;

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }

        }
        /// <summary>
        /// Update User Details
        /// </summary>
        /// <param name="userViewModel"></param>
        public bool UpdateUserDetails(UserViewModel userViewModel,out string msg)
        {
            msg = "";
            try
            {
                if (userViewModel.AddressViewModel.Postcode != null)
                {
                    userViewModel.AddressID = (int)addressBL.CheckAddress(userViewModel.AddressViewModel);    //Assign a AddressID for Employee
                    userViewModel.AddressViewModel.AddressID = userViewModel.AddressID;
                    if (userViewModel.AddressViewModel.AddressID == 0)
                    {
                        msg = "AddressID Is Not Saved!.....";
                        return false;
                    }
                }
            var uusr = (from uu in db.User_UserDetails where uu.UserId == userViewModel.UserID select uu).SingleOrDefault();
            userViewModel.DOB = new DateTime(userViewModel.Year, userViewModel.Month, userViewModel.Day);
            if (uusr != null)
            {
                uusr.Email = userViewModel.Email;
                uusr.EmployeeNo = userViewModel.EmployeeNO;
                uusr.FirstName = userViewModel.FirstName;
                uusr.LastName = userViewModel.LastName;
                uusr.AddressID = userViewModel.AddressID;
                uusr.UserName = userViewModel.Username;
                uusr.Mobile = userViewModel.Mobile;
                uusr.Telephone = userViewModel.Telephone;
                uusr.TitleID = (int)userViewModel.TitleID;
                uusr.JoinDate = userViewModel.JoinDate;
                uusr.JobTitleID = (short)userViewModel.JobTitleID;
                uusr.StatusComment = userViewModel.StatusComment;
                uusr.Status = userViewModel.Status;
                uusr.DateofBirth = userViewModel.DOB;
                db.SaveChanges();
                    msg = "Successfully Updated!";
            }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// Get Job Title List
        /// </summary>
        /// <returns></returns>
        public List<Common_JobTitle> GetJobTitle()
        {
            return (from ct in db.Common_JobTitle select ct).ToList();
        }
        /// <summary>
        /// GET Title
        /// </summary>
        /// <returns></returns>
        public List<Common_Title> GetTitle()
        {
            return (from cti in db.Common_Title select cti).ToList();
        }
        /// <summary>
        /// Check EmployeeID
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public bool CheckEmployeeID(string employee)
        {
            var chkemp = db.User_UserDetails.Where(m => m.EmployeeNo == employee).Select(m => m).Count();
            return chkemp == 0 ? true : false;
        }
        /// <summary>
        /// Check Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckUsername(string username)
        {
            var chkusr = db.User_UserDetails.Where(m => m.UserName == username).Select(m => m).Count();
            return chkusr == 0 ? true : false;
        }
        /// <summary>
        /// Get Username by UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetUsernameByID(string userID)
        {
            var usrname = (from uusr in db.User_UserDetails where uusr.UserId == userID select uusr.UserName).FirstOrDefault();
            return usrname;
        }
        /// <summary>
        /// Get Employeeno By UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetEmployeeByID(string userID)
        {
            var emp = (from uusr in db.User_UserDetails where uusr.UserId == userID select uusr.EmployeeNo).FirstOrDefault();
            return emp;
        }
        /// <summary>
        /// Check Email Address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool CheckEmail(string email)
        {
            var chkmail = db.AspNetUsers.Where(x => x.Email == email).Select(x => x).Count();
            return chkmail == 0 ? true : false;
        }
        /// <summary>
        /// Check User For Remote Validation
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckUser(string username, string userID)
        {
            var usr = db.User_UserDetails.Where(x => x.UserName.ToLower() == username && x.UserId != userID).FirstOrDefault() != null;
            return usr;
        }
        /// <summary>
        /// Check Employee No For Remote Validation
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public bool CheckEmp(string emp, string userID)
        {
            var em = db.User_UserDetails.Where(x => x.EmployeeNo.ToLower() == emp && x.UserId != userID).FirstOrDefault() != null;
            return em;
        }
        /// <summary>
        /// Check Role For Remote Validation
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool CheckRole(string role)
        {
            var rol = db.AspNetRoles.Where(x => x.Name.ToLower() == role).FirstOrDefault() != null;
            return rol;
        }
        /// <summary>
        /// Get User List
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserList()
        {
            return db.User_UserDetails.Select(x => x.UserName).ToList();
        }
        public List<User_UserDetails> GetUserDetailsList()
        {
            return db.User_UserDetails.ToList();
        }
    }
}
