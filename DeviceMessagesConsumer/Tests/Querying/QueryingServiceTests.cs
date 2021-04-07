using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using DeviceMessagesConsumer.DataAccess;
using DeviceMessagesConsumer.DataAccess.Entities;
using DeviceMessagesConsumer.Querying;
using FluentAssertions;
using NUnit.Framework;

namespace DeviceMessagesConsumer.Tests.Querying
{
    [TestFixture]
    public class QueryingServiceTests
    {
        private QueryingService service;
        private IMapper mapper;
        private DbConnection connection;
        private IFixture fixture;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }

        [SetUp]
        public void Setup()
        {
            fixture = DeviceMeasurementsFixture.Create();

            connection = Effort.DbConnectionFactory.CreateTransient();

            var context = new DeviceMeasurementsContext();
            context.Database.CreateIfNotExists();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            mapper = config.CreateMapper();
            Func<DeviceMeasurementsContext> dbContextFactory = () => CreateDbContext();
            service = new QueryingService(dbContextFactory, mapper);
        }

        [Test]
        public async Task Should_Get_Statistics()
        {
            // arrange
            var device1 = await CreateDevicesWithMeasurementsAsync();
            var device2 = await CreateDevicesWithMeasurementsAsync();

            // act
            var result = await service.GetStatisticsAsync();

            // assert
            result.Count.Should().Be(2);
            device1.Should().BeEquivalentTo(result.First(), options => options.ExcludingMissingMembers());
            device2.Should().BeEquivalentTo(result.Last(), options => options.ExcludingMissingMembers());
        }

        private async Task<Device> CreateDevicesWithMeasurementsAsync()
        {
            var context = CreateDbContext();

            var device = fixture.Create<Device>();
            device.Measurements = fixture.CreateMany<Measurement>().ToList();

            context.Devices.Add(device);

            await context.SaveChangesAsync();

            return device;
        }

        private DeviceMeasurementsContext CreateDbContext()
        {
            var context = new DeviceMeasurementsContext(connection);
            context.Database.CreateIfNotExists();

            return context;
        }
    }
}