using System;

namespace ClientSimulator.Models
{
    public class DeviceMeasurementCreateModel
    {
        public MeasuredParameterType MeasuredParameterType { get; set; }

        public float Value { get; set; }

        public DateTimeOffset MeasuredAt { get; set; }
    }
}