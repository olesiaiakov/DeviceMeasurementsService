using System.Data.Entity.Migrations;

namespace DeviceMessagesConsumer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DeviceMessagesConsumer.DataAccess.DeviceMeasurementsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}