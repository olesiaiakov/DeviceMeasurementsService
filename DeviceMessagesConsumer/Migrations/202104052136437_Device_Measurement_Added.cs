namespace DeviceMessagesConsumer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Device_Measurement_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Short(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Measurements",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MeasuredParameterType = c.Int(nullable: false),
                        Value = c.Single(nullable: false),
                        MeasuredAt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        DeviceId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.DeviceId, cascadeDelete: true)
                .Index(t => new { t.DeviceId, t.MeasuredParameterType, t.MeasuredAt }, unique: true);

            Sql(@"exec('
                SET IDENTITY_INSERT [DeviceMeasurements].[dbo].[Devices] ON

                INSERT INTO [dbo].[Devices] ([Id], [Name] ,[IsActive]) VALUES (1 ,''Sensor 1'', 1)
                INSERT INTO [dbo].[Devices] ([Id], [Name] ,[IsActive]) VALUES (2 ,''Sensor 2'', 1)
                INSERT INTO [dbo].[Devices] ([Id], [Name] ,[IsActive]) VALUES (3 ,''Sensor 3'', 1)

                SET IDENTITY_INSERT [DeviceMeasurements].[dbo].[Devices] OFF
            ')");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Measurements", "DeviceId", "dbo.Devices");
            DropIndex("dbo.Measurements", new[] { "DeviceId", "MeasuredParameterType", "MeasuredAt" });
            DropTable("dbo.Measurements");
            DropTable("dbo.Devices");
        }
    }
}
