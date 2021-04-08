using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;
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
            CheckDeviceMeasuredParameterType(model);

            using (var ownedFactory = dbContextFactory())
            {
                var dbContext = ownedFactory;

                var device = await dbContext.Devices.SingleOrDefaultAsync(d => d.Id == deviceId);
                if (device == null)
                {
                    ThrowBadRequestError($"Received message from unknown device '{deviceId}'");
                }

                if (!device.IsActive)
                {
                    ThrowBadRequestError($"Received message from inactive device '{deviceId}'");
                }

                var deviceMeasurements = mapper.Map<ICollection<Measurement>>(model);
                
                var now = DateTimeOffset.UtcNow;
                foreach (var deviceMeasurement in deviceMeasurements)
                {
                    deviceMeasurement.DeviceId = deviceId;
                    deviceMeasurement.CreatedAt = now;
                }

                dbContext.Measurements.AddRange(deviceMeasurements);

                try
                {
                    await dbContext.SaveChangesAsync();
                    Log.Information("Device '{DeviceId}': {Count} measurement(s) added", deviceId, deviceMeasurements.Count);
                }
                catch (DbUpdateException e) when (e.IsKeyViolation())
                {
                    ThrowBadRequestError($"Not possible to save measures per device '{deviceId}' with same DeviceId, MeasuredParameterType, MeasuredAt");
                }
            }
        }

        private void CheckDeviceMeasuredParameterType(ICollection<DeviceMeasurementCreateModel> model)
        {
            foreach (var item in model)
            {
                if (!Enum.IsDefined(typeof(MeasuredParameterType), item.MeasuredParameterType))
                {
                    ThrowBadRequestError($"Received message with incorrect MeasuredParameterType '{item.MeasuredParameterType}'");
                }
            }
        }

        private void ThrowBadRequestError(string error)
        {
            Log.Warning(error);
            
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent(error);
            
            throw new HttpResponseException(response);
        }
    }
}