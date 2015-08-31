﻿using System.Web;
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
                      "~/Content/Site.css"
                      ));
        }
    }
}
