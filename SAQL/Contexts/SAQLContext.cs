using Microsoft.EntityFrameworkCore;
using SAQL.Entities;

namespace SAQL.Contexts
{
    public class SAQLContext : DbContext
    {
        public SAQLContext()
        {

        }
        public SAQLContext(DbContextOptions<SAQLContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=dbsaql.database.windows.net;Database=SaqlDB;Trusted_Connection=False;Encrypt=True;User Id=sqladmin;Password=sql!Admin");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasOne(a => a.Device)
                .WithOne(b => b.Patient)
                .HasForeignKey<Device>(b => b.PatientId);

            modelBuilder.Entity<Device>()
                .HasOne(d => d.Patient)
                .WithOne(p => p.Device)
                .HasForeignKey<Patient>(p => p.DeviceId);
        }


        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patron> Patrons { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<PhysiologicalData> PhysiologicalData { get; set; }
    }
}
