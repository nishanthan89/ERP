using ERP.Admin.BL;
using ERP.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ERP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ERPEntities db = new ERPEntities();
        public static string DateFormat="" ;
        public static string TimeFormat = "";
        public static string TimeZone = "";
        public static string Currency = "";
        //public static string DateFormat = "yyyy-MM-dd";

        public static void Globalvariables(){
             GlobalApplicationVariables globalApplicationVariables = new GlobalApplicationVariables();
             DateFormat = globalApplicationVariables.GetCommonDate();
            TimeFormat = globalApplicationVariables.GetCommonTime();
            TimeZone = globalApplicationVariables.GetCommonTimeZone();
            Currency = globalApplicationVariables.GetCurrency();

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Globalvariables();

        }
    }
}
