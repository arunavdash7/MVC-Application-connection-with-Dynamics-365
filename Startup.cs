using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestAppMVCAndCRM.Startup))]
namespace TestAppMVCAndCRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
