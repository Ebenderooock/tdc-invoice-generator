// Copyright 2024 - Eben de Roock
// Created: 2024${File.CreatedMonth}${File.CreatedDay} @11:16

using System.Configuration;

namespace InvoiceGenerator.Core.Helper
{
    public static class Settings
    {
        public static string InvoiceTemplatePath => ConfigurationManager.AppSettings["InvoiceTemplate"];
        public static string ReportTemplatePath => ConfigurationManager.AppSettings["ReportTemplate"];
        public static string ConfigKey => ConfigurationManager.AppSettings["ConfigKey"];
        public static string LogoUrl = ConfigurationManager.AppSettings["LogoUrl"];
        public static string LogoCenterUrl = ConfigurationManager.AppSettings["LogoUrl_Center"];
        public static string BackgroundImageUrl = ConfigurationManager.AppSettings["BackgroundImage"];
        public static string IconUrl = ConfigurationManager.AppSettings["Icon"];
        public static string ApplicationTitle = ConfigurationManager.AppSettings["ApplicationTitle"];
        public static string CompanyName = ConfigurationManager.AppSettings["CompanyName"];
    }
}