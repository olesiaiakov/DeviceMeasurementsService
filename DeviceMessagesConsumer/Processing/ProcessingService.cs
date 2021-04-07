using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;
using DeviceMessagesConsumer.DataAccess;
using DeviceMessagesConsumer.DataAccess.Entities;
using Serilog;

namespace DeviceMessagesConsumer.Processing
{
    internal class ProcessingService : IProcessingService
    {
        private readonly Func<DeviceMeasurementsContext> dbContextFactory;
        private readonly IMapper mapper;

        public ProcessingService(Func<DeviceMeasurementsContext> dbContext, IMapper mapper)
        {
            this.dbContextFactory = dbContext;
            this.mapper = mapper;
        }

        public async Task CreateAsync(short deviceId, ICollection<DeviceMeasurementCreateModel> model)
        {
            using (var ownedFactory = dbContextFactory())
            {
                var dbContext = ownedFactory;

                var device = await dbContext.Devices.SingleOrDefaultAsync(d => d.Id == deviceId);
                if (device == null)
                {
                    Log.Warning("Received message from unknown device '{DeviceId}'", deviceId);
                    throw new InvalidOperationException("Device with such id was not found");
                }

                if (!device.IsActive)
                {
                    Log.Warning("Received message from inactive device '{DeviceId}'", deviceId);
                    throw new InvalidOperationException("Device is not active anymore");
                }

                var deviceMeasurements = mapper.Map<ICollection<Measurement>>(model);
                var now = DateTimeOffset.UtcNow;
                foreach (var deviceMeasurement in deviceMeasurements)
                {
                    deviceMeasurement.DeviceId = deviceId;
                    deviceMeasurement.CreatedAt = now;
                }

                dbContext.Measurements.AddRange(deviceMeasurements);

                await dbContext.SaveChangesAsync();

                Log.Information("Device '{DeviceId}': {Count} measurement(s) added", deviceId, deviceMeasurements.Count);
            }
        }
    }
}