using AutoMapper;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;
using DeviceMessagesConsumer.DataAccess.Entities;

namespace DeviceMessagesConsumer.Processing
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DeviceMeasurementCreateModel, Measurement>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.CreatedAt, opts => opts.Ignore())
                .ForMember(dest => dest.DeviceId, opts => opts.Ignore())
                .ForMember(dest => dest.Device, opts => opts.Ignore());
        }
    }
}