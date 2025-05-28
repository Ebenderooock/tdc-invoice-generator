namespace InvoiceGenerator2.Models
{

    public sealed class ApplicationConfiguration
    {
        public string InvoiceTemplatePath { get; set; } = string.Empty;
        public string ReportTemplatePath { get; set; } = string.Empty;
        public string ConfigKey { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string LogoCenterUrl { get; set; } = string.Empty;
        public string BackgroundImageUrl { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string ApplicationTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string PassPhrase { get; set; } = string.Empty;
    }
}