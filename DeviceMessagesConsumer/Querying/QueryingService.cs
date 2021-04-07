using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;
using DeviceMessagesConsumer.DataAccess;
using Serilog;

namespace DeviceMessagesConsumer.Querying
{
    internal class QueryingService : IQueryingService
    {
        private readonly Func<DeviceMeasurementsContext> dbContextFactory;
        private readonly IMapper mapper;

        public QueryingService(Func<DeviceMeasurementsContext> dbContext, IMapper mapper)
        {
            this.dbContextFactory = dbContext;
            this.mapper = mapper;
        }

        public async Task<ICollection<DeviceModel>> GetStatisticsAsync()
        {
            using (var ownedFactory = dbContextFactory())
            {
                Log.Information("Statistics requested");
                var dbContext = ownedFactory;
                var devices = await dbContext.Devices
                    .Include(d => d.Measurements)
                    .Where(d => d.IsActive)
                    .ToListAsync();
                return mapper.Map<ICollection<DeviceModel>>(devices);
            }
        }
    }
}