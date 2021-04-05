using System.Web.Http;
using DeviceMessagesConsumer;
using WebActivatorEx;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace DeviceMessagesConsumer
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "DeviceMessagesConsumer");
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }
}
