using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InvoiceGenerator_Core.Startup))]
namespace InvoiceGenerator_Core
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
