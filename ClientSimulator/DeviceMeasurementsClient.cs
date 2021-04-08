using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ClientSimulator.Models;
using Flurl;
using Flurl.Http;
using Serilog;

namespace ClientSimulator
{
    public class DeviceMeasurementsClient
    {
        private readonly string resourceUrl;

        private readonly Random randomizer = new Random();

        public DeviceMeasurementsClient(string resourceUrl)
        {
            this.resourceUrl = resourceUrl;
        }

        public async Task StartSendingRequestsAsync(short deviceId, int period)
        {
            while (true)
            {
                var model = CreateRandomDeviceMeasurements();

                await SendRequestAsync(deviceId, model);

                await Task.Delay(period);
            }
        }

        private ICollection<DeviceMeasurementCreateModel> CreateRandomDeviceMeasurements()
        {
            ICollection<DeviceMeasurementCreateModel> result = new List<DeviceMeasurementCreateModel>();

            var count = randomizer.Next(1, Enum.GetValues(typeof(MeasuredParameterType)).Length + 1);

            for (var i = 0; i < count; i++)
            {
                result.Add(new DeviceMeasurementCreateModel
                {
                    MeasuredParameterType = (MeasuredParameterType)i,
                    Value = (float)randomizer.NextDouble(),
                    MeasuredAt = DateTimeOffset.UtcNow,
                });
            }

            return result;
        }

        private async Task SendRequestAsync(short deviceId, ICollection<DeviceMeasurementCreateModel> model)
        {
            try
            {
                await resourceUrl
                    .AppendPathSegment("devices")
                    .AppendPathSegment(deviceId)
                    .AppendPathSegment("measurements")
                    .PostJsonAsync(model);

                Log.Information("Device {deviceId} sent a request with {modelCount} measurement(s)", deviceId, model.Count);
            }
            catch (FlurlHttpTimeoutException)
            {
                Log.Error("Timeout occured");
            }
            catch (FlurlHttpException e)
            {
                Log.Error("Error returned from {url}:", e.Call.Request.Url);
                if (e.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    var error = await e.GetResponseJsonAsync();
                    Log.Error(error.ExceptionMessage);
                }
                else
                {
                    Log.Error(e.GetBaseException().Message);
                }
            }
        }
    }
}