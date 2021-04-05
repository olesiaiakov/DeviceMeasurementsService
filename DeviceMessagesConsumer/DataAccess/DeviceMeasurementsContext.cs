using System.Data.Entity;
using DeviceMessagesConsumer.DataAccess.Entities;

namespace DeviceMessagesConsumer.DataAccess
{
    public class DeviceMeasurementsContext : DbContext
    {
        public DeviceMeasurementsContext() : base("mssql")
        {
        }
        
        public DbSet<Device> Devices { get; set; }
         
        public DbSet<Measurement> Measurements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Measurement>()
                .HasRequired(m => m.Device)
                .WithMany(d => d.Measurements)
                .HasForeignKey(m => m.DeviceId);

            modelBuilder.Entity<Measurement>().HasIndex(m => new { m.DeviceId, m.MeasuredParameterType, m.MeasuredAt }).IsUnique();
        }
    }
}