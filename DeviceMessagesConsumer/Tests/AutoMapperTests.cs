using AutoMapper;
using NUnit.Framework;

namespace DeviceMessagesConsumer.Tests
{
    [TestFixture]
    public class AutoMapperTests
    {
        [Test]
        public void Should_Be_Valid()
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<DeviceMessagesConsumer.Querying.MappingProfile>();
                c.AddProfile<DeviceMessagesConsumer.Processing.MappingProfile>();
            });

            var mapper = config.CreateMapper();

            Assert.DoesNotThrow(() => mapper.ConfigurationProvider.AssertConfigurationIsValid());
        }
    }
}