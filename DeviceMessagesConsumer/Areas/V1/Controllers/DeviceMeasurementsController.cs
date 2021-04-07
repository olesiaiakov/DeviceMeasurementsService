using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;
using DeviceMessagesConsumer.Processing;
using DeviceMessagesConsumer.Querying;

namespace DeviceMessagesConsumer.Areas.V1.Controllers
{
    [RoutePrefix("api/v1/device-measurements")]
    public class DeviceMeasurementsController : ApiController
    {
        private readonly IProcessingService processingService;
        private readonly IQueryingService queryingService;

        public DeviceMeasurementsController(IProcessingService processingService, IQueryingService queryingService)
        {
            this.processingService = processingService;
            this.queryingService = queryingService;
        }

        [HttpPost]
        [Route("devices/{deviceId}/measurements")]
        public Task Create(short deviceId, ICollection<DeviceMeasurementCreateModel> model)
        {
            return processingService.CreateAsync(deviceId, model);
        }

        [HttpGet]
        [Route("devices/statistics")]
        public Task<ICollection<DeviceModel>> GetStatisticsAsync()
        {
            return queryingService.GetStatisticsAsync();
        }
    }
}