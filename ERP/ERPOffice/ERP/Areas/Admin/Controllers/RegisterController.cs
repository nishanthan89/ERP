//--------------------------------------------------------------------------------
// <copyright file="RegisterController.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

// <Author>
//      M Thinesh 
// </Author>
//--------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ERP.MvcSecurity;
using ERP;
using ERP.Admin.ViewModels;
using ERP.Admin.BL;
using ERP.Address.BL;
using ERP.DA;
using ERP.Models;

namespace ERP.Areas.Admin.Controllers
{
    [Authorize]
    [AuthorizeUserAttribute(Permission = 5)]
    public class RegisterController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        //private UserPermissionView userPermissionView = new UserPermissionView();
        RegisterBL registerBL = new RegisterBL();
        AddressBL addressBL = new AddressBL();
        private PermissionBL permissionBL = new PermissionBL();
        private AddressBL addressBl = new AddressBL();  //Gets all methods from AddressBL
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        /// <summary>
        /// GET: Admin/Register
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View(new UserViewModel());
        }
        /// <summary>
        /// Viewbag List
        /// </summary>
        public void _ViewDetails()
        {
            ViewBag.TitleList = (registerBL.GetTitle().Select(x => new SelectListItem()
            {
                Value = x.TitleID.ToString(), Text = x.TitleName
            }));
            ViewBag.JobTitleList = (registerBL.GetJobTitle().Select(x => new SelectListItem()
            {
                Value = x.JobTitleID.ToString(), Text = x.JobTitleName
            }));
            //For Country List
            ViewBag.CountryList = addressBl.GetCountryList().Select(x => new SelectListItem()
            {
                Value = x.CountryID.ToString(),
                Text = x.CountryName,
                Selected = x.IsSelected
            });      //Gets Country List
        }
        /// <summary>
        /// GET: /Account/Register
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute(Permission =1)]
        public ActionResult Register()
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.AddressViewModel = new Address.ViewModels.AddressViewModel();
            userViewModel.AddressViewModel.CountryID= addressBL.GetDefaultCountry();
            _ViewDetails();
            return View(userViewModel);
        }

        /// <summary>
        ///  POST: /Account/Register
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute(Permission =1)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserViewModel model)
        {
            string msg = "";
            if (model.UserID != null)
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }
            if (ModelState.IsValid)
            {

                if (model.UserID == null)
                {
                    if (registerBL.CheckEmployeeID(model.EmployeeNO) == false)
                    {
                        ModelState.AddModelError("", "Employee No Already Exists");
                        _ViewDetails();
                        return View(model);
                    }
                    var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        model.UserID = user.Id;
                        if (registerBL.CreateUser(model, out msg))
                        {
                            //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                            // Send an email with this link
                            // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                            // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                            //return RedirectToAction("Index", "Home", new { area = "" });
                            TempData["Success"] = msg;
                        }
                        else
                        {
                            ModelState.AddModelError("", msg);
                            return View(model);
                        }
                        return RedirectToAction("UserPermission", "UserPermission", new { area = "Admin" });
                    }
                    AddErrors(result);
                }
                else {
                    if (registerBL.CheckEmployeeID(model.EmployeeNO) == false && model.EmployeeNO != registerBL.GetEmployeeByID(model.UserID))
                    {
                        ModelState.AddModelError("", "Employee No Already Exists");
                        _ViewDetails();
                        return View(model);
                    }
                    var userUpdate = await UserManager.FindByIdAsync(model.UserID);
                    if (userUpdate != null)
                    {
                        userUpdate.Email = model.Email;
                        userUpdate.UserName = model.Username;
                        var updateresult = await UserManager.UpdateAsync(userUpdate);
                        if (updateresult.Succeeded)
                        {
                            if (registerBL.UpdateUserDetails(model, out msg))
                            {
                                TempData["Success"] = "Successfully Updated!";
                            }
                            else
                            {
                                ModelState.AddModelError("", msg);
                                _ViewDetails();
                                return View(model);
                            }
                            return RedirectToAction("UserPermission", "UserPermission", new { area = "Admin" });
                        }
                        AddErrors(updateresult);
                    }
                    else {
                        return RedirectToAction("UserPermission", "UserPermission", new { area = "Admin" });
                    }
                }
            }
            _ViewDetails();
            //var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
           // TempData["Error"] = message;
            return View(model);
        }
        
        /// <summary>
        /// DeleteUser
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteUser(string id)
        {
            string msg = "";
            if (id != null)
            {
                if (registerBL.DeleteUserDetails(id, out msg))
                {
                    TempData["Success"] = "Successfully Deleted!";
                }
            }
            return RedirectToAction("UserPermission", "UserPermission");

        }
        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditUser(string id)
        {
            string msg = "";
            if (id != null)
            {
                var model = registerBL.EditUserDetails(id,out msg);
                _ViewDetails();
                if (model != null)
                {
                    return View("Register", model);
                }
                else
                {
                    TempData["Error"] = msg;
                }
            }
            return RedirectToAction("UserPermission", "UserPermission");
        }
        /// <summary>
        /// Remote Validation Username
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult IsUserExist(string userName, string userID)
        {
            bool isExist= registerBL.CheckUser(userName, userID);
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///  Remote Validation Email
        /// </summary>
        /// <param name="employeeNO"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult IsEmpExist(string employeeNO, string userID)
        {
            bool isExist = registerBL.CheckEmp(employeeNO, userID);
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Typeahead()
        {
            var  Usr = registerBL.GetUserList();
            return Json(Usr.ToArray(), JsonRequestBehavior.AllowGet);
        }


    }
}