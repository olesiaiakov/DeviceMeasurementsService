using Autofac;
using DeviceMessagesConsumer.Processing;

namespace DeviceMessagesConsumer
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<ProcessingService>().As<IProcessingService>();

            return builder.Build();
        }
    }
}