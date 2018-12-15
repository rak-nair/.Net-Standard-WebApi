using AssignmentAPI.Data;
using AssignmentAPI.Services;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Web;
using System.Web.Http;

namespace AssignmentAPI
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register(() =>
            {
                return new AssignmentDbContext();
            }, container.Options.DefaultScopedLifestyle);

            container.Register<IAssignmentData, SQLAssignmentData>();

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
