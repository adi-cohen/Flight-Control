using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightManager
    {
        private readonly FlightPlanManager flightPlanManager;
        private readonly DBInteractor db;

        public FlightManager(FlightPlanManager flightPlanManager, DBInteractor newDB)
        {
            this.db = newDB;
            this.flightPlanManager = flightPlanManager;
        }
        public List<Flight> GetAllFlights(DateTime time)
        {
            //get all active flights plans according to time
           var ActiveFlights = flightPlanManager.GetActiveFlights(time);

            //create flights list
            var flightList = new List<Flight>();
            foreach (FlightPlan fp in ActiveFlights)
            {
                double lastLongitude = db.InitLocations.Where(i => i.FlightId == fp.Id).First().Longitude;
                double lastLatitude = db.InitLocations.Where(i => i.FlightId == fp.Id).First().Latitude;
                DateTime lastStrartTime = db.InitLocations.Where(i => i.FlightId == fp.Id).First().DateTime;

                var segList = flightPlanManager.GetFlightPlanSegments(fp.Id);
                foreach (Segment seg in segList)
                {
                    DateTime newEndTime = lastStrartTime.AddSeconds(seg.TimeInSeconds);
                    if (newEndTime < time)
                    {
                        //move to the next segment
                        lastLongitude = seg.Longitude;
                        lastLatitude = seg.Latitude;
                        lastStrartTime = newEndTime;
                    }
                    else //The requested time is in the segment
                    {
                        TimeSpan theDiffFromStartTime = time - lastStrartTime;
                        double theDiffFromStartTimeInSeconds = (double)theDiffFromStartTime.TotalSeconds;
                        double precentOfTime = Math.Abs(theDiffFromStartTimeInSeconds) / Math.Abs(seg.TimeInSeconds);
                        double startLongitude = lastLongitude;
                        double startLatitude = lastLatitude;
                        double endLongitude = seg.Longitude;
                        double endlatitude = seg.Latitude;
                        //Use Pythagorean Theorem to determine segment length
                        double distanceOfAllSegment = Math.Sqrt(Math.Pow((endlatitude - startLatitude), 2) + Math.Pow((endLongitude - startLongitude), 2));
                        double distanceFromStartUntilTime = distanceOfAllSegment * precentOfTime;
                        //Calculate the angle to calculate the new position
                        double angle = Math.Acos((Math.Abs(startLongitude - endLongitude)) / distanceOfAllSegment);
                        double LatitudeDiff = precentOfTime * distanceOfAllSegment * Math.Sin(angle);
                        double LongitudeDiff = precentOfTime * distanceOfAllSegment * Math.Cos(angle);
                        double newLatitude, newLongitude;
                        if (startLatitude < endlatitude)
                        {
                            newLatitude = startLatitude + LatitudeDiff;
                        }
                        else
                        {
                            newLatitude = startLatitude - LatitudeDiff;
                        }
                        if (startLongitude < endLongitude)
                        {
                            newLongitude = startLongitude + LongitudeDiff;
                        }
                        else
                        {
                            newLongitude = startLongitude - LongitudeDiff;
                        }
                        var newFlight = new Flight
                        {
                            FlightId = fp.Id,
                            CompanyName = fp.CompanyName,
                            Passengers = fp.Passengers,
                            IsExternal = false,
                            Latitude = newLatitude,
                            Longitude = newLongitude,
                            Date = lastStrartTime.AddSeconds(theDiffFromStartTimeInSeconds)
                        };
                        flightList.Add(newFlight);
                        break;
                    }
                }
            }

            return flightList;
        }

        public string RemoveFlight(string id)
        {
            var flightPlan = db.FlightPlans.Find(id);

            if (flightPlan == null)
            {
                return null;
            }

            //delete all the segment of the flight

            var segmentToDelete = db.Segments.Where(e => e.FlightId == id).ToList();
            foreach (Segment s in segmentToDelete)
            {
                db.Segments.Remove(s);
            }

            //delete the initial location of the flight
            var initLocationToDelete = db.InitLocations.Where(e => e.FlightId == id).First();
            db.InitLocations.Remove(initLocationToDelete);

            //delete the flight plan
            db.FlightPlans.Remove(flightPlan);
            db.SaveChangesAsync();
            return flightPlan.Id;

        }

    }
}
