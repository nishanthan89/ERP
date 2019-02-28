using System.Web;
using System.Web.Optimization;

namespace ERP
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
              "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery.validate.js",
                    //"~/Scripts/jquery-2.0.0.min.js",
                      "~/Scripts/jquery.freezeheader.js",
                      "~/Scripts/jquery.unobtrusive-ajax.js",
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/jquery-ui-1.10.2.js",
                      "~/Scripts/chosen.jquery.min.js",
                      "~/Scripts/jquery.tablesorter.min.js",
                      "~/Scripts/localScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                      "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                          "~/Scripts/bootstrap.js",
                          "~/Scripts/bootstrap-datepicker.js",
                          "~/Scripts/moment.min.js",
                          "~/Scripts/respond.js",
                          "~/Scripts/dist/js/app.min.js",
                          "~/Scripts/sweet-alert.js",
                          "~/Scripts/fullcalendar.min.js",
                          "~/Scripts/fastclick.min.js",
                          "~/Scripts/jquery.slimscroll.min.js",
                          "~/Scripts/jsLocal.js",
                          "~/Scripts/bootstrap2-toggle.min.js",
                          "~/Scripts/bootstrap-timepicker.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js").Include(
                   "~/Scripts/dist/js/demo.js"
                    ));

            bundles.Add(new StyleBundle("~/bootstrap/css").Include(
                         "~/bootstrap/css/bootstrap.css",
                          "~/Content/bootstrap-datepicker.min.css",
                         "~/Content/site.css",
                         "~/Content/bootstrap2-toggle.min.css",
                          "~/Content/font-awesome.min.css",
                          "~/Content/jquery.dataTables.min.css",
                          "~/Content/sweetalert/sweet-alert.css",
                  "~/Content/bootstrap-timepicker.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                         "~/Content/dist/css/skins/_all-skins.min.css",
                        "~/Content/dist/css/AdminLTE.css",
                        "~/Content/fullcalendar.min.css",
                        "~/Content/chosen.min.css"));


        }
    }
}
