using System.Web;
using System.Web.Optimization;

namespace InvoiceGenerator_Core
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/mandatory").Include(
                        "~/Vendor/jquery/jquery-3.3.1.min.js",
                        "~/Vendor/jquery-ui/jquery-ui.min.js",
                        "~/Vendor/moment/moment.js",
                        "~/Vendor/bootstrap/js/bootstrap.bundle.min.js",
                        "~/Vendor/slimscroll/jquery.slimscroll.min.js",
                        "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/css/mandatory").Include(
                      "~/Vendor/bootstrap/css/bootstrap.min.css",
                      "~/Vendor/jquery-ui/jquery-ui.min.css",
                      "~/Vendor/jquery-ui/jquery-ui.theme.min.css",
                      "~/Vendor/simple-line-icons/css/simple-line-icons.css",
                      "~/Vendor/fontawesome/css/all.min.css",
                      "~/Content/Main.css"));

            bundles.Add(new StyleBundle("~/css/custom").Include(
                      "~/Content/Site.css"));

            // Datatable Plugin Scripts and CSS
            bundles.Add(new ScriptBundle("~/js/datatable").Include(
                        "~/Vendor/datatable/js/jquery.dataTables.min.js",
                        "~/Vendor/datatable/js/dataTables.bootstrap4.min.js",
                        "~/Vendor/datatable/pdfmake/pdfmake.min.js",
                        "~/Vendor/datatable/pdfmake/vfs_fonts.js",
                        "~/Vendor/datatable/buttons/js/dataTables.buttons.min.js",
                        "~/Vendor/datatable/buttons/js/buttons.bootstrap4.min.js",
                        "~/Vendor/datatable/buttons/js/buttons.colVis.min.js",
                        "~/Vendor/datatable/buttons/js/buttons.flash.min.js",
                        "~/Vendor/datatable/buttons/js/buttons.html5.min.js",
                        "~/Vendor/datatable/buttons/js/buttons.print.min.js",
                        "~/Vendor/search-highlight/jquery-highlight/js/jquery.highlight.js",
                        "~/Vendor/search-highlight/datatables-search-highlight/js/dataTables.searchHighlight.min.js"));

            bundles.Add(new StyleBundle("~/css/datatable").Include(
                      "~/Vendor/datatable/css/dataTables.bootstrap4.min.css",
                      "~/Vendor/datatable/buttons/css/buttons.bootstrap4.min.css"));

            // Switches Plugin Scripts and CSS
            bundles.Add(new ScriptBundle("~/js/switches").Include(
                        "~/Vendor/bootstrap4-toggle/js/bootstrap4-toggle.min.js"));

            bundles.Add(new StyleBundle("~/css/switches").Include(
                      "~/Vendor/bootstrap4-toggle/css/bootstrap4-toggle.min.css"));

            // Tagify Plugin Scripts and CSS
            bundles.Add(new ScriptBundle("~/js/tagify").Include(
                        "~/Vendor/tagify/js/jQuery.tagify.min.js"));

            bundles.Add(new StyleBundle("~/css/tagify").Include(
                      "~/Vendor/tagify/css/tagify.css"));

            // Datepicker Plugin Scripts and CSS
            bundles.Add(new ScriptBundle("~/js/flatpickr").Include(
                        "~/Vendor/flatpickr/js/flatpickr.js"));

            bundles.Add(new StyleBundle("~/css/flatpickr").Include(
                      "~/Vendor/flatpickr/css/flatpickr.min.css"));

            // Select2 Plugin Scripts and CSS
            bundles.Add(new ScriptBundle("~/js/select2").Include(
                        "~/Vendor/select2/js/select2.min.js"));

            bundles.Add(new StyleBundle("~/css/select2").Include(
                      "~/Vendor/select2/css/select2.min.css",
                      "~/Vendor/select2/css/select2-bootstrap.min.css"));

            // Pages
            // Users 
            bundles.Add(new ScriptBundle("~/js/users/index").Include(
                        "~/Scripts/Pages/Users/Index.js"));

            bundles.Add(new ScriptBundle("~/js/users/edit").Include(
                        "~/Scripts/Pages/Users/Edit.js"));

            bundles.Add(new ScriptBundle("~/js/users/delete").Include(
                        "~/Scripts/Pages/Users/Delete.js"));

            // Products 
            bundles.Add(new ScriptBundle("~/js/products/index").Include(
                        "~/Scripts/Pages/Products/Index.js"));

            bundles.Add(new ScriptBundle("~/js/products/edit").Include(
                        "~/Scripts/Pages/Products/Edit.js"));

            bundles.Add(new ScriptBundle("~/js/products/delete").Include(
                        "~/Scripts/Pages/Products/Delete.js"));
            bundles.Add(new ScriptBundle("~/js/products/upload").Include(
                        "~/Scripts/Pages/Products/Upload.js"));

            // Clients 
            bundles.Add(new ScriptBundle("~/js/clients/index").Include(
                        "~/Scripts/Pages/Clients/Index.js"));

            bundles.Add(new ScriptBundle("~/js/clients/edit").Include(
                        "~/Scripts/Pages/Clients/Edit.js"));
            bundles.Add(new ScriptBundle("~/js/clients/upload").Include(
                        "~/Scripts/Pages/Clients/Upload.js"));

            bundles.Add(new ScriptBundle("~/js/clients/delete").Include(
                        "~/Scripts/Pages/Clients/Delete.js"));

            // Invoices 
            bundles.Add(new ScriptBundle("~/js/invoices/index").Include(
                        "~/Scripts/Pages/Invoices/Index.js"));

            bundles.Add(new ScriptBundle("~/js/invoices/create").Include(
                        "~/Scripts/Pages/Invoices/Create.js"));

            bundles.Add(new ScriptBundle("~/js/invoices/details").Include(
                        "~/Scripts/Pages/Invoices/Details.js"));

            bundles.Add(new ScriptBundle("~/js/invoices/edit").Include(
                        "~/Scripts/Pages/Invoices/Edit.js"));

            bundles.Add(new ScriptBundle("~/js/invoices/delete").Include(
                        "~/Scripts/Pages/Invoices/Delete.js"));

            // Invoice Dashboard 
            bundles.Add(new ScriptBundle("~/js/dashboard/invoices").Include(
                        "~/Vendor/chartjs/js/chart.min.js",
                        "~/Vendor/apexcharts/js/apexcharts.min.js",
                        "~/Scripts/Pages/Dashboard/Invoices/Dashboard.js"));

        }
    }
}