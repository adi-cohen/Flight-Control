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

            ExternalFlight flight1 = new ExternalFlight() { FlightId = "WUWA41", ExternalServerUrl = "http://ronyut2.atwebpages.com/ap2" };
            modelBuilder.Entity<ExternalFlight>().HasData(flight1);


            /* InitialLocation initLocation1 = new InitialLocation() { Longitude = 31.2, Latitude = 34.2, DateTime = DateTime.Now };
             InitialLocation initLocation12 = new InitialLocation() { Longitude = 31.7, Latitude = 34.5, DateTime = DateTime.Now };

             modelBuilder.Entity<FlightPlan>().HasData(
             new FlightPlan() { Id = 1, Passengers = 4, CompanyName = "combo", InitialLocation =  initLocation1, Segments = null },
             new FlightPlan() { Id = 2, Passengers = 3, CompanyName = "mmba", InitialLocation =initLocation12, Segments = null }
             );*/

            /*modelBuilder.Entity<Server>().HasData(
            new Server() { Id = 1, Url = "testURL.com" }
            );*/

        }
    }
}

