using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using DeviceMessagesConsumer.DataAccess;
using DeviceMessagesConsumer.Processing;
using DeviceMessagesConsumer.Querying;

namespace DeviceMessagesConsumer
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<Processing.MappingProfile>();
                c.AddProfile<Querying.MappingProfile>();
            });

            var mapper = configuration.CreateMapper();

            builder
                .RegisterType<ProcessingService>()
                .WithParameter(new TypedParameter(typeof(IMapper), mapper))
                .As<IProcessingService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<QueryingService>()
                .WithParameter(new TypedParameter(typeof(IMapper), mapper))
                .As<IQueryingService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<DeviceMeasurementsContext>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}