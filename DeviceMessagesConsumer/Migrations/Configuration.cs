using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

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