using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using DeviceMessagesConsumer.Areas.V1.Controllers.Models;

namespace DeviceMessagesConsumer.DataAccess.Entities
{
    public class Measurement
    {
        public long Id { get; set; }
        
        public MeasuredParameterType MeasuredParameterType { get; set; }

        public float Value { get; set; }
        
        public DateTimeOffset MeasuredAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        
        public short DeviceId { get; set; }
        
        public Device Device { get; set; }
    }
}