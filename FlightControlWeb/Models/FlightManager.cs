﻿using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightManager
    {
        private IFlightPlanManager flightPlanManager;
        private  DBInteractor db;

        public FlightManager(IFlightPlanManager flightPlanManager, DBInteractor newDb)
        {
            this.db = newDb;
            this.flightPlanManager = flightPlanManager;
        }
        public List<Flight> getAllFlights (DateTime time)
        {
            //get all active flights plans according to time
            List<FlightPlan> ActiveFlights = flightPlanManager.GetActiveFlights(time);

            //create flights list
            List<Flight> flightList = new List<Flight>();
            foreach (FlightPlan fp in ActiveFlights)
            {
                double lastLongitude = db.InitLocations.Where(i => i.FlightId == fp.Id).First().Longitude;
                double lastLatitude = db.InitLocations.Where(i => i.FlightId == fp.Id).First().Latitude;
                DateTime lastStrartTime = db.InitLocations.Where(i => i.FlightId == fp.Id).First().DateTime;

                List<Segment> segList = flightPlanManager.GetFlightPlanSegments(fp.Id);
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
                        int theDiffFromStartTimeInSeconds = theDiffFromStartTime.Seconds;
                        int precentOfTime = Math.Abs(theDiffFromStartTimeInSeconds) / Math.Abs(seg.TimeInSeconds);
                        double startLongitude = lastLongitude;
                        double startLatitude = lastLatitude;
                        double endLongitude = seg.Longitude;
                        double endlatitude = seg.Latitude;
                        //Use Pythagorean Theorem to determine segment length
                        double distanceOfAllSegment = Math.Pow((endlatitude - startLatitude), 2) + Math.Pow((endLongitude - startLongitude), 2);
                        double distanceFromStartUntilTime = distanceOfAllSegment * precentOfTime;
                        //Calculate the angle to calculate the new position
                        double angle = Math.Acos((Math.Abs(startLongitude - endLongitude)) / distanceOfAllSegment);
                        double LatitudeDistance = precentOfTime * distanceOfAllSegment * Math.Cos(angle);
                        double LongitudeDistance = precentOfTime * distanceOfAllSegment * Math.Sin(angle);








                        double longitudeDiff = Math.Abs(seg.Longitude - lastLongitude);
                        double latitudeDiff = Math.Abs(seg.Latitude - lastLatitude);
                       // int precentOfTimeSpent ;
                        //TimeSpan theDiffFromStartTime = time - lastStrartTime;
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
                        newFlight.Date = lastStrartTime.AddSeconds(DiffInSeconds);
                        flightList.Add(newFlight);
                    }

                }
            }

            return flightList;
        }

        public long? RemoveFlight (int id)
        {
            FlightPlan flightPlan =  db.FlightPlans.Find(id);

            if (flightPlan == null)
            {
                return null ;
            }

            //delete all the segment of the flight

            List<Segment> segmentToDelete = db.Segments.Where(e => e.FlightId == id).ToList();
            foreach (Segment s in segmentToDelete)
            {
                db.Segments.Remove(s);
            }

            //delete the initial location of the flight
            InitialLocation initLocationToDelete = db.InitLocations.Where(e => e.FlightId == id).First();
            db.InitLocations.Remove(initLocationToDelete);

            //delete the flight plan
            db.FlightPlans.Remove(flightPlan);
            db.SaveChangesAsync();
            return flightPlan.Id;

        }

    }
}