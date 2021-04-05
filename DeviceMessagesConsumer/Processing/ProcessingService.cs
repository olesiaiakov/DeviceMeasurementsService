using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using AutoMapper;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;
using DeviceMessagesConsumer.DataAccess;
using DeviceMessagesConsumer.DataAccess.Entities;
using Serilog;

namespace DeviceMessagesConsumer.Processing
{
    internal class ProcessingService : IProcessingService
    {
        private readonly Func<Owned<DeviceMeasurementsContext>> dbContextFactory;
        private readonly IMapper mapper;
        
        public ProcessingService(Func<Owned<DeviceMeasurementsContext>> dbContext, IMapper mapper)
        {
            this.dbContextFactory = dbContext;
            this.mapper = mapper;
        }

        public async Task CreateAsync(short deviceId, ICollection<DeviceMeasurementCreateModel> model)
        {
            using (var ownedFactory = dbContextFactory())
            {
                var dbContext = ownedFactory.Value;

                var device = await dbContext.Devices.SingleOrDefaultAsync(d => d.Id == deviceId);
                if (device == null)
                {
                    throw new InvalidOperationException("Device with such id was not found");
                }
                
                if (!device.IsActive)
                {
                    throw new InvalidOperationException("Device is not active anymore");
                }

                var deviceMeasurements = mapper.Map<ICollection<Measurement>>(model);
                foreach (var deviceMeasurement in deviceMeasurements)
                {
                    deviceMeasurement.DeviceId = deviceId;
                }

                dbContext.Measurements.AddRange(deviceMeasurements);
                
                await dbContext.SaveChangesAsync();

                Log.Information("Device {DeviceId} measurements were added", deviceId);
            }
        }
    }
}