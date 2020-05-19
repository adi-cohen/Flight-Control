using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FlightControlWeb;
using FlightControlWeb.Models;
    
namespace FlightControlWeb.Models
{
    public class DBInteractor : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseSqlite(@"Data Source = FlightControl.db;");
        }
        public DbSet<FlightPlan> FlightPlans { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<InitialLocation> InitLocations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Segment>().ToTable("Segments");
            modelBuilder.Entity<FlightPlan>().ToTable("FlightPlan");
            modelBuilder.Entity<InitialLocation>().ToTable("InitialLocation");

            InitialLocation initLocation1 = new InitialLocation() { Longitude = 31.2, Latitude = 34.2, DateTime = DateTime.Now };
            InitialLocation initLocation12 = new InitialLocation() { Longitude = 31.7, Latitude = 34.5, DateTime = DateTime.Now };

            modelBuilder.Entity<FlightPlan>().HasData(
            new FlightPlan() { Id = 1, Passengers = 4 , CompanyName = "combo",  InitialLocation = initLocation1, Segments =null},
            new FlightPlan() { Id = 2, Passengers = 3, CompanyName = "mmba",  InitialLocation = initLocation12, Segments = null}
            );
            
        }
        public DbSet<FlightControlWeb.Models.Flight> Flight { get; set; }
    }
}

