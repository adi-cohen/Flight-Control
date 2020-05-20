using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightManager
    {
        private IFlightPlanManager flightPlanManager;
        private readonly DBInteractor db;

        FlightManager(IFlightPlanManager flightPlanManager)
        {
            this.flightPlanManager = flightPlanManager;
        }
        public List<Flight> getAllFlights (DateTime time)
        {
            List<FlightPlan> ActiveFlights = flightPlanManager.GetActiveFlights(time);
            List<Flight> flightList = new List<Flight>();
            foreach (FlightPlan fp in ActiveFlights)
            {
                double lastLongitude = db.InitLocations.Where(i => i.FlightId == fp.Id).First().Longitude;
                double lastLatitude = db.InitLocations.Where(i => i.FlightId == fp.Id).First().Latitude;
                DateTime lastTime = db.InitLocations.Where(i => i.FlightId == fp.Id).First().DateTime;


                List<Segment> segList = flightPlanManager.GetFlightPlanSegments(fp.Id);
                foreach (Segment seg in segList)
                {
                    DateTime newEndTime = lastTime.AddSeconds(seg.TimeInSeconds);
                    if (newEndTime < time)
                    {
                        //move to the next segment
                        lastLongitude = seg.Longitude;
                        lastLatitude = seg.Latitude;
                        lastTime = newEndTime;
                    }
                    else // the time is in the segment
                    {
                        double longitudeDiff = seg.Longitude - lastLongitude;
                        double latitudeDiff = seg.Latitude - lastLatitude;
                        TimeSpan theDiffFromStartTime = time - lastTime;
                        int DiffInSeconds = theDiffFromStartTime.Seconds;
                        double precentFromSegment = (DiffInSeconds / seg.TimeInSeconds);
                        double newLongitude = lastLongitude + (longitudeDiff * precentFromSegment);
                        double newLatitude = lastLatitude + (latitudeDiff * precentFromSegment);
                        Flight newFlight = new Flight();
                        newFlight.FlightId = fp.Id ;
                        newFlight.CompanyName = fp.CompanyName;
                        newFlight.Passengers = fp.Passengers;
                        newFlight.IsExternal = false;
                        newFlight.Latitude = newLatitude;
                        newFlight.Longitude = newLongitude;
                        newFlight.Date = lastTime.AddSeconds(DiffInSeconds);
                        flightList.Add(newFlight);
                    }

                }
            }

            return flightList;
        }
    }
}
