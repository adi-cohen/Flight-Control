using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanManager
    {
        private readonly DBInteractor db;
        private readonly IdGenerator generator;

        public FlightPlanManager(DBInteractor newDB)
        {
            db = newDB;
            generator = new IdGenerator(db);

        }
        public void AddFlightPlan(FlightPlan flightplan)
        {
            db.FlightPlans.Add(flightplan);
            db.SaveChanges();
        }

        public void DeleteFlightPlan(int id)
        {
            var flightToRemove = db.FlightPlans.Find(id);
            db.FlightPlans.Remove(flightToRemove);
            db.SaveChanges();
        }


        public FlightPlan GetFlightPlan(int id)
        {
            var flight = db.FlightPlans.Find(id);
            return flight;
        }

        public List<FlightPlan> GetActiveFlights(DateTime time)
        {
            var relativeFlightPlan = new List<FlightPlan>();
            foreach (FlightPlan flight in db.FlightPlans)
            {
                var segmentList = GetFlightPlanSegments(flight.Id);
                var startAndEndTime = GetStartAndEndTime(flight.Id, segmentList);
                // if the flight active in the requierd time
                if (startAndEndTime[0] <= time && startAndEndTime[1] >= time)
                {
                    relativeFlightPlan.Add(flight);
                }
            }
            return relativeFlightPlan;
        }

        public List<Segment> GetFlightPlanSegments(string flightPlanID)
        {
            var segList = new List<Segment>();
            segList = db.Segments.Where(s => s.FlightId == flightPlanID).ToList();
            segList.OrderBy(s => s.Id);
            return db.Segments.Where(s => s.FlightId == flightPlanID).OrderBy(s => s.SegmentNumber).ToList();

        }

        private List<DateTime> GetStartAndEndTime(string flightID, List<Segment> segmentsList)
        {
            long seconds = 0;
            //sum all the seconds during the flight
            foreach (Segment s in segmentsList)
            {
                seconds += s.TimeInSeconds;
            }
            //adding the seconds to the startTime
            var initLocation = db.InitLocations.Where(l => l.FlightId == flightID).First();
            DateTime endTime = initLocation.DateTime;
            endTime = endTime.AddSeconds(seconds);
            var startAndEndTime = new List<DateTime>()
            {
                initLocation.DateTime, endTime
            };
            return startAndEndTime;
        }



        public FlightPlan CreateNewFlightPlan(FlightPlan flightPlan)
        {
            //generate random id
            string FlightId = generator.GanerateId();
            flightPlan.Id = FlightId;
            db.FlightPlans.Add(flightPlan);

            //adding all segments to DB
            int segmentNum = 1;
            foreach (Segment element in flightPlan.Segments)
            {
                //adding segment number for segments in the same flight plan
                element.SegmentNumber = segmentNum;
                segmentNum++;
                element.FlightId = FlightId;
                element.Id = generator.GanerateId();
                db.Segments.Add(element);
            }

            //adding InitialLocation to DB
            flightPlan.InitialLocation.FlightId = FlightId;
            flightPlan.InitialLocation.Id = generator.GanerateId();
            db.InitLocations.Add(flightPlan.InitialLocation);

            db.SaveChanges();
            return flightPlan;
        }
    }
}
