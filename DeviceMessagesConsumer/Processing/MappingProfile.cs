using AutoMapper;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;
using DeviceMessagesConsumer.DataAccess.Entities;

namespace DeviceMessagesConsumer.Processing
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DeviceMeasurementCreateModel, Measurement>(MemberList.Source);
        }
    }
}