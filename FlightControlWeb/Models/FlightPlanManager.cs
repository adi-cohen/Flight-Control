using Microsoft.AspNetCore.Authentication.OAuth.Claims;
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


        public FlightPlan GetFlightPlan(int id)
        {
            FlightPlan flight = db.FlightPlans.Find(id);
            return flight;
        }

        public List<FlightPlan> GetActiveFlights(DateTime time)
        {
            List<FlightPlan> relativeFlightPlan = new List<FlightPlan>();
            foreach (FlightPlan flight in db.FlightPlans){
                List<Segment> segmentList = GetFlightPlanSegments(flight.Id);
                List<DateTime> startAndEndTime = GetStartAndEndTime(flight.Id, segmentList);
                // if the flight active in the requierd time
                if (startAndEndTime[0] <= time && startAndEndTime[1] >= time)
                {
                    relativeFlightPlan.Add(flight);
                }
            }
            return relativeFlightPlan;
        }

        public List<Segment> GetFlightPlanSegments(long flightPlanid)
        {
            List<Segment> segList = new List<Segment>();
            segList = db.Segments.Where(s => s.FlightId == flightPlanid).ToList();
            segList.OrderBy(s => s.Id);
            return db.Segments.Where(s => s.FlightId == flightPlanid).OrderBy(s => s.SegmentNumber).ToList();

        }

        private List<DateTime> GetStartAndEndTime(long flightId, List<Segment> segList)
        {
            long seconds = 0;
            //sum all the seconds during the flight
            foreach(Segment s in segList)
            {
                seconds += s.TimeInSeconds;
            }
            //adding the seconds to the startTime
            InitialLocation initLocation= db.InitLocations.Where(l => l.FlightId == flightId).First();
            DateTime endTime = initLocation.DateTime;
            endTime = endTime.AddSeconds(seconds);
            List<DateTime> startAndEndTime = new List<DateTime>()
            {
                initLocation.DateTime, endTime
            };
            return startAndEndTime;
        }

        public long GanerateID()
        {
            Random rand = new Random();
            long id = rand.Next(10000, 999999999);
            return id;

        }

      
    }
}
