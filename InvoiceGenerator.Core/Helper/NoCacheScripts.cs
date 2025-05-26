// Copyright 2024 - Eben de Roock
// Created: 2024${File.CreatedMonth}${File.CreatedDay} @22:35

using System;
using System.Web;
using System.Web.Optimization;

namespace InvoiceGenerator.Core.Helper
{
    public static class NoCacheScripts
    {
        public static IHtmlString Render(string scriptPath)
        {
            return Scripts.Render($"{scriptPath}?v={DateTime.UtcNow.Ticks}");
        }
    }
}