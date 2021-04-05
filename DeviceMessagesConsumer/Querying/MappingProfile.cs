using AutoMapper;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;
using DeviceMessagesConsumer.DataAccess.Entities;

namespace DeviceMessagesConsumer.Querying
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Device, DeviceModel>()
                .ForMember(dest => dest.MeasuresCount, opt => opt.MapFrom(src => src.Measurements.Count));
        }
    }
}