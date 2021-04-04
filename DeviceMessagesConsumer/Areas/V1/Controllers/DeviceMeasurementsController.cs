using System.Threading.Tasks;
using System.Web.Http;
using DeviceMessagesConsumer.Processing;

namespace DeviceMessagesConsumer.Areas.V1.Controllers
{
    public class DeviceMeasurementsController : ApiController
    {
        private readonly IProcessingService processingService;

        public DeviceMeasurementsController(IProcessingService processingService)
        {
            this.processingService = processingService;
        }

        [HttpPost]
        public string Create()
        {
            return "test";
        }
    }
}