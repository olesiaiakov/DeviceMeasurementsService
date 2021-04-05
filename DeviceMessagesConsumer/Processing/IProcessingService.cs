using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;

namespace DeviceMessagesConsumer.Processing
{
    public interface IProcessingService
    {
        Task CreateAsync(short deviceId, ICollection<DeviceMeasurementCreateModel> model);
    }
}