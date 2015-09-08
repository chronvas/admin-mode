using System.Web;
using System.Web.Optimization;

namespace admin_mode
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js" 
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusive-ajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusive-ajaxmin").Include(
            "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //bootstrap switch
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-switch").Include(
                                    "~/Scripts/bootstrap-switch.js*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-switchmin").Include(
                                    "~/Scripts/bootstrap-switch.min.js*"));

            //bootstrap multiselect
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-multiselect").Include(
                        "~/Scripts/bootstrap-multiselect.js",
                        "~/Scripts/bootstrap-multiselect-collapsible-groups"
                        ));

            //bootstrap datepicker
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
                        "~/Scripts/bootstrap-datepicker.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker-min").Include(
                        "~/Scripts/bootstrap-datepicker-min.js"
                        ));

            //bootstrap datetimepicker
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetimepicker").Include(
                        "~/Scripts/moment-with-locales.js" ,
                        "~/Scripts/bootstrap-datetimepicker.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetimepicker-min").Include(
                        "~/Scripts/moment-with-locales.min.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js"
                        ));
           
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/modalform").Include(
                      "~/Scripts/modalform.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/gpyphed-icon-list.css",
                      "~/Content/bootstrap-switch/bootstrap3/bootstrap-switch.css",
                      "~/Content/bootstrap-multiselect.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/Site.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/ForLogin").Include("~/Content/ForLogin.css"));

        }
    }
}
