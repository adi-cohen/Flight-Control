using Microsoft.EntityFrameworkCore;

namespace FlightControlWeb.Models
{
    public class DBInteractor : DbContext
    {

        public DbSet<FlightPlan> FlightPlans { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<InitialLocation> InitLocations { get; set; }
        public DbSet<ExternalFlight> ExternalFlights { get; set; }
        public DbSet<IdNumber> IdNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseSqlite(@"Data Source = FlightControl.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Segment>().ToTable("Segments");
            modelBuilder.Entity<FlightPlan>().ToTable("FlightPlan");
            modelBuilder.Entity<InitialLocation>().ToTable("InitialLocation");
            modelBuilder.Entity<Server>().ToTable("Servers");
            modelBuilder.Entity<ExternalFlight>().ToTable("ExternalFlights");
            modelBuilder.Entity<IdNumber>().ToTable("IdNumbers");

        }
    }
}

