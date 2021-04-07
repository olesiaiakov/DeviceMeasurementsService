using System.Web.Http;
using Autofac.Integration.WebApi;
using Owin;

namespace DeviceMessagesConsumer
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host.
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            SwaggerConfig.Register(config);

            var container = ContainerConfig.Configure();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(config);
            appBuilder.UseWebApi(config);
        }
    }
}