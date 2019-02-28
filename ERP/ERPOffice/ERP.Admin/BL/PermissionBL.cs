using ERP.Admin.ViewModels;
using ERP.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERP.Admin.BL
{
    public class PermissionBL
    {
        private ERPEntities db = new ERPEntities();
        //private UserPermissionView userPermissionView = new UserPermissionView();
        /// <summary>
        /// Create User Permission Details
        /// </summary>
        /// <param name="userPermissionView"></param>
        public void CreateUserPermission(UserRoleViewModel userRoleViewModel)
        {
            foreach (var item in userRoleViewModel.SelectPermissions)
            {
                var per = (from auth in db.AspNetPermissions where (auth.PermissionID == item) select auth).FirstOrDefault();
                var rol = (from role in db.AspNetRoles where (role.Name == userRoleViewModel.RoleName) select role).FirstOrDefault();
                rol.AspNetPermissions.Add(per);
            }
            db.SaveChanges();
        }
        /// <summary>
        /// Get the User List
        /// </summary>
        /// <returns></returns>
        public List<User_UserDetails> GetUserName()
        {
            var ulist = (from uusr in db.User_UserDetails where uusr.UserName!="sysadmin" select uusr).ToList();
            return ulist;
        }
        /// <summary>
        /// Get the User Permission
        /// </summary>
        /// <returns></returns>
        public List<AspNetPermission> GetPermission()
        {
            return (from per in db.AspNetPermissions select per).ToList();
        }

        /// <summary>
        ///  Get permission list by application
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IEnumerable<int>[] GetUserAssignedPermissions(string username)
        {
            IEnumerable<int>[] permssionList = (from r in db.AspNetRoles
                                                where r.AspNetUsers.Any(x => x.UserName == username)
                                                select r.AspNetPermissions.Select(x => x.PermissionID)).ToArray();


            return permssionList;

        }
        /// <summary>
        /// Get Login By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public LogoutViewModel GetLoginByUserId(string userId)
        {
            LogoutViewModel loginLogout = null;
            try
            {
                loginLogout = (from l in db.AspNetLoginOffs.Where(a => a.UserID == userId)
                               orderby l.Login descending
                               select new LogoutViewModel()
                               {
                                   LoginLogoutID = l.LoginLogoutID,
                                   UserID = l.UserID,
                                   Login = l.Login,
                                   DeviceID = l.DeviceId,
                               }).FirstOrDefault();

                return loginLogout;
            }
            catch
            {
                return loginLogout;
            }
        }
        /// <summary>
        /// Update User Session
        /// </summary>
        /// <param name="loginLogout"></param>
        public void UpdateUserSession(LogoutViewModel loginLogout)
        {
            try
            {
                if (loginLogout.UserID != null) //update data while user id available 
                {
                    AspNetLoginOff aspLoginLogout = (from l in db.AspNetLoginOffs.Where(a => a.UserID == loginLogout.UserID)
                                                     orderby l.Login descending
                                                     select l).FirstOrDefault();
                    aspLoginLogout.Login = loginLogout.Login;
                    db.SaveChanges();
                }
            }
            catch { }
        }
        /// <summary>
        /// Save User Session
        /// </summary>
        /// <param name="loginLogout"></param>
        public void SaveUserSession(LogoutViewModel loginLogout)
        {
            if (loginLogout.UserID != null)
            {
                AspNetLoginOff aspLoginLogout = new AspNetLoginOff();
                aspLoginLogout.UserID = loginLogout.UserID;
                aspLoginLogout.Login = loginLogout.Login;
                db.AspNetLoginOffs.Add(aspLoginLogout);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Remove Logout User
        /// </summary>
        /// <param name="remUser"></param>
        public bool RemoveLogoutUser(string remUser,out string msg)
        {
            msg = "";
            try
            {
                if (remUser != null)
                {
                    var curUser = db.AspNetLoginOffs.Where(x => x.UserID == remUser).ToList();
                    if (curUser != null)
                    {
                        curUser.ForEach(p =>
                        {
                            db.AspNetLoginOffs.Remove(p);
                        });
                        db.SaveChanges();
                        msg = "Your Acoount has been Logged out!";
                    }
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
        /// Get Logout UserID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetLogoutUserID(string id)
        {
            string remUser = db.AspNetLoginOffs.Where(x => x.UserID == id).Select(x=>x.UserID).FirstOrDefault();
            return String.IsNullOrEmpty(remUser) ? true : false;
        }
        public List<AspNetRole> GetRoleList()
        {
            return db.AspNetRoles.ToList();
        }
        public void DBSaveChange()
        {
            db.SaveChanges();
        }
    }
}
