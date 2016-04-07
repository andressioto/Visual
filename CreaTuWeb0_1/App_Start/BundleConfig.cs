using System.Web.Optimization;

namespace IdentitySample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //VOY A CREAR MIS PROPIOS BUNDLES PARA MI PLANTILLA


            //bondles para css
            bundles.Add(new StyleBundle("~/Content/mios").Include(
            "~/Content/mios/bootstrap.css",
            "~/Content/mios/font-awesome.min.css",
            "~/Content/mios/jquery.bxslider.css",
            "~/Content/mios/style.css",
            "~/Content/mios/fine-uploader-new.css"));
            //bondles para js
            bundles.Add(new ScriptBundle("~/bundles/mios").Include(
              "~/Scripts/mios/jquery.bxslider.min.js",
              "~/Scripts/mios/jquery.blImageCenter.js",
              "~/Scripts/mios/Mio.js",
              "~/Scripts/mios/mimity.js"));
        }
    }
}
