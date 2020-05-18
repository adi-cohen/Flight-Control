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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlightPlan>().HasData(
            new FlightPlan() { Id = 1, Passengers = 4 , CompanyName = "combo", StartLongitude = 31.22, StartLatitude = 32.44, StartDate = DateTime.Now, Segments =null},
            new FlightPlan() { Id = 2, Passengers = 3, CompanyName = "mmba", StartLongitude = 31.44, StartLatitude = 32.33, StartDate = DateTime.Now, Segments = null}
            );
        }
    }
}

