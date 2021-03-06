﻿using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    internal class FlightPlanManager
    {
        private readonly DBInteractor db;
        private readonly IdGenerator generator;

        internal FlightPlanManager(DBInteractor newDB)
        {
            db = newDB;
            generator = new IdGenerator(db);

        }
        internal void AddFlightPlan(FlightPlan flightplan)
        {
            db.FlightPlans.Add(flightplan);
            db.SaveChanges();
        }

        internal void DeleteFlightPlan(int id)
        {
            var flightToRemove = db.FlightPlans.Find(id);
            db.FlightPlans.Remove(flightToRemove);
            db.SaveChanges();
        }


        internal FlightPlan GetFlightPlan(int id)
        {
            var flight = db.FlightPlans.Find(id);
            return flight;
        }

        internal List<FlightPlan> GetActiveFlights(DateTime time)
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

        internal List<Segment> GetFlightPlanSegments(string flightPlanID)
        {
            var segList = new List<Segment>();
            segList = db.Segments.Where(segment => segment.FlightId == flightPlanID).ToList();
            segList.OrderBy(segment => segment.Id);
            return db.Segments.Where(segment => segment.FlightId == flightPlanID).OrderBy(segment => segment.SegmentNumber).ToList();

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
            var initLocation = db.InitLocations.Where(initialLocation => initialLocation.FlightId == flightID).First();
            DateTime endTime = initLocation.DateTime;
            endTime = endTime.AddSeconds(seconds);
            var startAndEndTime = new List<DateTime>()
            {
                initLocation.DateTime, endTime
            };
            return startAndEndTime;
        }


        internal bool IsFlightValid(FlightPlan flightPlan)
        {
            if (flightPlan.InitialLocation == null ||
                flightPlan.Segments == null)
            {
                return false;
            }
            foreach (Segment seg in flightPlan.Segments)
            {
                if (seg.Latitude < -90 || seg.Latitude > 90)
                {
                    return false;
                }
                if (seg.Longitude < -180 || seg.Longitude > 180)
                {
                    return false;
                }
                if (seg.TimeInSeconds == 0)
                {
                    return false;
                }
            }
            if (flightPlan.InitialLocation.Longitude < -180 ||
                flightPlan.InitialLocation.Longitude > 180 ||
                flightPlan.InitialLocation.Latitude < -90 ||
                flightPlan.InitialLocation.Latitude > 90)
            {
                return false;
            }
            return true;
        }
        internal FlightPlan CreateNewFlightPlan(FlightPlan flightPlan)
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
