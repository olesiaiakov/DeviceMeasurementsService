using AutoFixture;
using DeviceMessagesConsumer.DataAccess.Entities;

namespace DeviceMessagesConsumer.Tests
{
    internal static class DeviceMeasurementsFixture
    {
        public static IFixture Create()
        {
            var fixture = new Fixture();

            fixture.Customize<Device>(d => d
                .Without(m => m.Measurements)
                .With(m => m.IsActive, true));

            fixture.Customize<Measurement>(m => m
                .Without(d => d.Device));

            return fixture;
        }
    }
}