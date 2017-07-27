using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestExamMaker.Startup))]
namespace TestExamMaker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
