using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;

namespace DeviceMessagesConsumer.Querying
{
    public interface IQueryingService
    {
        Task<ICollection<DeviceModel>> GetStatisticsAsync();
    }
}