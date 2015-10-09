using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SapeMorale.Model.Startup))]
namespace SapeMorale.Model
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
