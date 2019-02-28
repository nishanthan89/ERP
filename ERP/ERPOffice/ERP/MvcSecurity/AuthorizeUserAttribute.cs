
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ERP.Admin.BL;

namespace ERP.MvcSecurity
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public int Permission { get; set; }
        private PermissionBL permissionBL = new PermissionBL();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            
            var uID = HttpContext.Current.User.Identity.GetUserId();
            if(permissionBL.GetLogoutUserID(uID) ==true)
            {
                httpContext.Response.Redirect("~/Account/LogOffUser");
                return false;
            }
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            HttpCookie permis = httpContext.Request.Cookies["Permissions"];
            if (permis == null)
            {
                if(httpContext.User.Identity.Name=="sysadmin")
                {
                    return true;
                }
                httpContext.Response.Redirect("~/Account/AccessDenied");
                return false;
            }

            string pr = permis.Value;
            if (!pr.Equals(string.Empty))
            {
                int[] Permissions = permis.Value.Split(',').Select(p => Convert.ToInt32(p)).ToArray();

                if (Permissions.Contains(1))
                {
                    return true;
                }
                else if (Permissions.Contains(this.Permission))
                {
                    return true;
                }
                else
                {
                    httpContext.Response.Redirect("~/Account/AccessDenied");
                    return false;
                }
            }
            else
            {
                httpContext.Response.Redirect("~/Account/AccessDenied");
                return false;
            }
        }
    }
}