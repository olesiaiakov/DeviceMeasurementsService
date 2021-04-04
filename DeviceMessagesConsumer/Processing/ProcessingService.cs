using DeviceMessagesConsumer.DataAccess;

namespace DeviceMessagesConsumer.Processing
{
    internal class ProcessingService : IProcessingService
    {
        private readonly DeviceMeasurementsContext dbContext;
        
        public ProcessingService(DeviceMeasurementsContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}