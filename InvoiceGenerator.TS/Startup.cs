using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InvoiceGenerator_Core.Startup))]
namespace InvoiceGenerator.TDC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
