namespace DeviceMessagesConsumer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Device_Measurements_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
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
                        DeviceId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.DeviceId, cascadeDelete: false)
                .Index(t => t.DeviceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Measurements", "DeviceId", "dbo.Devices");
            DropIndex("dbo.Measurements", new[] { "DeviceId" });
            DropTable("dbo.Measurements");
            DropTable("dbo.Devices");
        }
    }
}
