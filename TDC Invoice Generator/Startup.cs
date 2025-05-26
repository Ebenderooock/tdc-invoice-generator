using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TDC_Invoice_Generator.Startup))]
namespace TDC_Invoice_Generator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
