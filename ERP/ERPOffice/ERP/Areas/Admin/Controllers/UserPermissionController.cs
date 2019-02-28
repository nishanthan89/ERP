//--------------------------------------------------------------------------------
// <copyright file="UserPermissionController.cs" company="BDL">
//     Copyright (c) Business Dispatch Ltd.  All rights reserved.
// </copyright>

// <Author>
//      M Thinesh 
// </Author>
//--------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using ERP.MvcSecurity;
using ERP.Models;
using ERP.Admin.ViewModels;
using ERP.Admin.BL;
using ERP.Controllers;

namespace ERP.Areas.Admin.Controllers
{
    [Authorize]
    [AuthorizeUserAttribute(Permission = 5)]
    public class UserPermissionController : Controller
    {
        ApplicationDbContext context;
        //private TESEntities db = new TESEntities();
        //private UserPermissionView userPermissionView = new UserPermissionView();
        private PermissionBL permissionBL = new PermissionBL();
      //  ResourceBL resourceBL = new ResourceBL();
        private RegisterBL registerBL = new RegisterBL();
        public UserPermissionController()
        {
            context = new ApplicationDbContext();
        }
        /// <summary>
        ///  GET: Admin/IndexView
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// GET:Admin/UserPermissionView
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Permission()
        {
            UserPermissionView userPermissionView = new UserPermissionView();
            userPermissionView.userRoleViewModelList = new List<UserRoleViewModel>();
            userPermissionView.userRoleViewModel = new UserRoleViewModel();
            var rolelist = permissionBL.GetRoleList();
            rolelist.ForEach(q =>
            {
                UserRoleViewModel roleViewModel = new UserRoleViewModel();
                List<UserTagListModel> userTagList = new List<UserTagListModel>();
                roleViewModel.RoleName = q.Name;
                var allusers = context.Users.ToList();
                var users = allusers.Where(x => x.Roles.Select(role => role.RoleId).Contains(q.Id)).ToList();
                users.ForEach(p =>
                {
                    var uname = registerBL.GetUserDetailsList().Where(x => x.UserId == p.Id).Select(x => x.UserName).FirstOrDefault();
                    UserTagListModel userTagListModel = new UserTagListModel();
                    userTagListModel.UserID = p.Id;
                    userTagListModel.Username = uname;
                    userTagList.Add(userTagListModel);
                });
                var userVM = userTagList.Select(user => new UserTagListModel { UserID = user.UserID, Username = string.Join(", ", userTagList.Select(r => r.Username)) }).ToList();
                roleViewModel.UserList = userVM;

                var allper = permissionBL.GetPermission().Select(y => y).ToList();
                var per = allper.Where(y => y.AspNetRoles.Select(m => m.Id).Contains(q.Id)).ToList();
                var perVM = per.Select(perm => new PermissionListView { PermissionID = perm.PermissionID, Permission = string.Join(", ", per.Select(x => x.Permission)) }).ToList();
                roleViewModel.PermissionList = perVM;

                userPermissionView.userRoleViewModelList.Add(roleViewModel);
            });
            _MultiTagList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_RoleDetails", userPermissionView.userRoleViewModelList);
            }
            return View(userPermissionView);
        }
        
        /// <summary>
        /// GET:Admin/UserDetails View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserPermission()
        {
            UserPermissionView userPermissionView = new UserPermissionView();
            userPermissionView.userViewModel= registerBL.GetUserDetail();
            return View(userPermissionView);
        }
        /// <summary>
        /// User Search
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserPermission(string searchBy, string searchText)
        {
            UserPermissionView userPermissionView = new UserPermissionView();
            userPermissionView.userViewModel = registerBL.GetUserDetailSearch(searchBy, searchText);
            return PartialView("_UserDetails",userPermissionView.userViewModel);
        }
        /// <summary>
        /// Viewbag List 
        /// </summary>
        public void _MultiTagList()
        {
            ViewBag.UserList = new MultiSelectList(permissionBL.GetUserName(), "UserId", "UserName");
            ViewBag.PermissionList = new MultiSelectList(permissionBL.GetPermission(), "PermissionID", "Permission");
        }
        /// <summary>
        /// GET:Admin/_CreateRole
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateRole()
        {
            _MultiTagList();
            return PartialView("_CreateRole", new UserRoleViewModel());
        }
        /// <summary>
        ///  POST:Admin/_CreateRole
        /// </summary>
        /// <param name="userRoleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateRole(UserRoleViewModel userRoleViewModel)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var account = new AccountController();
            try
            {
                if (ModelState.IsValid)
                {
                    var checkRole = context.Roles.Select(x => x.Id).Contains(userRoleViewModel.RoleID);
                    if (checkRole == true)
                    {

                        var delerole = context.Roles.Where(d => d.Id == userRoleViewModel.RoleID).FirstOrDefault();
                        var allusers = context.Users.ToList();
                        var users = allusers.Where(x => x.Roles.Select(r => r.RoleId).Contains(delerole.Id)).ToList();
                        if (users != null)
                        {
                            users.ForEach(y => { userManager.RemoveFromRole(y.Id, userRoleViewModel.RoleName); });
                        }
                        if (userRoleViewModel.SelectUsers != null)
                        {
                            userRoleViewModel.SelectUsers.ForEach(c => { userManager.AddToRole(c, userRoleViewModel.RoleName); });
                        }
                        var allper = permissionBL.GetPermission().Select(y => y).ToList();
                        var perUser = allper.Where(y => y.AspNetRoles.Select(m => m.Id).Contains(delerole.Id)).ToList();
                        if (perUser != null)
                        {
                            perUser.ForEach(p =>
                            {
                                var per = (from auth in permissionBL.GetPermission() where (auth.PermissionID == p.PermissionID) select auth).FirstOrDefault();
                                var rol = (from role in permissionBL.GetRoleList() where (role.Name == userRoleViewModel.RoleName) select role).FirstOrDefault();
                                rol.AspNetPermissions.Remove(per);
                            });
                        }
                        if (userRoleViewModel.SelectPermissions != null)
                        {
                            userRoleViewModel.SelectPermissions.ForEach(q =>
                            {
                                var per = (from auth in permissionBL.GetPermission() where (auth.PermissionID == q) select auth).FirstOrDefault();
                                var rol = (from role in permissionBL.GetRoleList() where (role.Name == userRoleViewModel.RoleName) select role).FirstOrDefault();
                                rol.AspNetPermissions.Add(per);
                            });
                        }
                        permissionBL.DBSaveChange();
                         TempData["Success"] = "Successfully Updated!";
                        //return Json(new { url = Url.Action("Permission") });
                        return Json(new { success = true });
                    }
                    
                    if (rm.RoleExists(userRoleViewModel.RoleName))
                    {
                        _MultiTagList();
                        ModelState.AddModelError("RoleName", "Role Name Already Exists");
                        return PartialView("_CreateRole", userRoleViewModel);
                    }
                    context.Roles.Add(new IdentityRole()
                    {
                        Name = userRoleViewModel.RoleName
                    });
                    context.SaveChanges();

                    if (userRoleViewModel.SelectUsers != null)
                    {
                        userRoleViewModel.SelectUsers.ForEach(m => { userManager.AddToRole(m, userRoleViewModel.RoleName); });
                    }
                    if (userRoleViewModel.SelectPermissions != null)
                    {
                        permissionBL.CreateUserPermission(userRoleViewModel);
                    }
                    //TempData.Remove("Successfully Updated!");
                    TempData["Success"] = "Successfully Created!";
                    return Json(new { success = true });
                }
                _MultiTagList();
                ModelState.AddModelError("RoleName", "Please Enter the Role Name");
                return PartialView("_CreateRole",userRoleViewModel);

            }
            catch
            {
                return View(userRoleViewModel);
            }
        }
        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public ActionResult DeleteRole(string role )
        {
            try
            {
                var delerole = context.Roles.Where(d => d.Name == role).FirstOrDefault();
                if (delerole != null)
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var account = new AccountController();
                    var allper = permissionBL.GetPermission().Select(y => y).ToList();
                    var per = allper.Where(y => y.AspNetRoles.Select(m => m.Id).Contains(delerole.Id)).ToList();
                    per.ForEach(p =>
                   {
                       var permission = (from auth in permissionBL.GetPermission() where (auth.PermissionID == p.PermissionID) select auth).FirstOrDefault();
                       var roles = (from ro in permissionBL.GetRoleList() where (ro.Name == delerole.Name) select ro).FirstOrDefault();
                       roles.AspNetPermissions.Remove(permission);
                       permissionBL.DBSaveChange();
                   });
                    
                   context.Roles.Remove(delerole);
                    context.SaveChanges();
                    TempData["Success"] = "Successfully Deleted!";
                   // return Json(new { success = true });
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return Json(new { error = true });
            }
        }
        /// <summary>
        /// Edit Role
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public ActionResult EditRole(string RoleName)
        {
            var editrole = context.Roles.Where(d => d.Name == RoleName).FirstOrDefault();
            if (editrole != null)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var account = new AccountController();
                var allusers = context.Users.ToList();
                var users = allusers.Where(x => x.Roles.Select(r => r.RoleId).Contains(editrole.Id)).ToList();
                var allper =permissionBL.GetPermission().Select(y => y).ToList();
                var per = allper.Where(y => y.AspNetRoles.Select(m => m.Id).Contains(editrole.Id)).ToList();
                UserPermissionView userPermissionView = new UserPermissionView();
                userPermissionView.userRoleViewModel = new UserRoleViewModel();
                userPermissionView.userRoleViewModel.RoleName = RoleName;
                userPermissionView.userRoleViewModel.RoleID = editrole.Id;
                userPermissionView.userRoleViewModel.SelectUsers = new List<string>();
                users.ForEach(p => { userPermissionView.userRoleViewModel.SelectUsers.Add(p.Id); });
                userPermissionView.userRoleViewModel.SelectPermissions = new List<int>();
                per.ForEach(p => { userPermissionView.userRoleViewModel.SelectPermissions.Add(p.PermissionID); });
                _MultiTagList();
                return PartialView("_CreateRole", userPermissionView.userRoleViewModel);
            }
            return RedirectToAction("Permission");
        }
        /// <summary>
        /// Log Out User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute(Permission = 1)]
        public ActionResult LogOffUser(string id)
        {
            string msg = "";
            if (permissionBL.RemoveLogoutUser(id,out msg)==false)
            {
                TempData["Error"] = msg;
            }
            return RedirectToAction("UserPermission");
        }
        /// <summary>
        /// Remote Validation for Role NAme
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult IsRoleExist(string roleName)
        {
            bool isExist = registerBL.CheckRole(roleName);
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }

    }
}