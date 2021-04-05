using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;
using DeviceMessagesConsumer.DataAccess;
using DeviceMessagesConsumer.DataAccess.Entities;

namespace DeviceMessagesConsumer.Querying
{
    internal class QueryingService : IQueryingService
    {
        private readonly DeviceMeasurementsContext dbContext;
        private readonly IMapper mapper;

        public QueryingService(DeviceMeasurementsContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        
        public async Task<ICollection<DeviceModel>> GetStatisticsAsync()
        {
            var devices = await dbContext.Devices
                .Include(d => d.Measurements)
                .Where(d => d.IsActive)
                .ToListAsync();

            return mapper.Map<ICollection<DeviceModel>>(devices);
        }
    }
}