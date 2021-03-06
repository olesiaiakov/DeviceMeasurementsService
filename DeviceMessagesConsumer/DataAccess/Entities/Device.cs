using System.Collections.Generic;

namespace DeviceMessagesConsumer.DataAccess.Entities
{
    public class Device
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Measurement> Measurements { get; set; }
    }
}