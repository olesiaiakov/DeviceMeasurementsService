using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;
using DeviceMessagesConsumer.DataAccess;
using DeviceMessagesConsumer.DataAccess.Entities;
using DeviceMessagesConsumer.Processing;
using FluentAssertions;
using NUnit.Framework;
using MappingProfile = DeviceMessagesConsumer.Processing.MappingProfile;

namespace DeviceMessagesConsumer.Tests.Processing
{
    [TestFixture]
    public class ProcessingServiceTests
    {
        private ProcessingService service;

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

            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            mapper = config.CreateMapper();

            Func<DeviceMeasurementsContext> dbContextFactory = () => CreateDbContext();
            service = new ProcessingService(dbContextFactory, mapper);
        }

        [Test]
        public async Task Should_Create()
        {
            // arrange
            var device = await CreateDeviceAsync();
            var request = fixture.CreateMany<DeviceMeasurementCreateModel>().ToList();

            // act
            Func<Task> act = async () => await service.CreateAsync(device.Id, request);

            // assert
            act.Should().NotThrow();

            var dbContext = CreateDbContext();
            var created = await dbContext.Devices.Where(d => d.Id == device.Id).Include(d => d.Measurements).SingleAsync();

            created.Measurements.Should().NotBeNull().And.HaveCount(request.Count);
            created.Id.Should().Be(device.Id);
        }

        private async Task<Device> CreateDeviceAsync()
        {
            var context = CreateDbContext();

            var device = fixture.Create<Device>();

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