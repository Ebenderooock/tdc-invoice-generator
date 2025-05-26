using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InvoiceGenerator.TS.Startup))]
namespace InvoiceGenerator.TS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
