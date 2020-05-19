using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanManager : IFlightPlanManager
    {
        private readonly DBInteractor db;

        public FlightPlanManager(DBInteractor newDb)
        {
            db = newDb;
        }
        public void AddFlightPlan(FlightPlan flightplan)
        {
            db.FlightPlans.Add(flightplan);
            db.SaveChanges();
        }

        public void DeleteFlightPlan(int id)
        {
            FlightPlan flightToRemove = db.FlightPlans.Find(id);
            db.FlightPlans.Remove(flightToRemove);
            db.SaveChanges();
        }

        public IEnumerable<FlightPlan> GetAllFlights()
        {
            return db.FlightPlans.ToList();
        }

        public FlightPlan GetFlightPlan(int id)
        {
            FlightPlan flight = db.FlightPlans.Find(id);
            return flight;
        }

        public IEnumerable<FlightPlan> GetInternalFlights()
        {
            throw new NotImplementedException();
        }

        public long GanerateID()
        {
            Random rand = new Random();
            long id = rand.Next(10000, 999999999);
            return id;

        }
    }
}
