using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightControlWeb.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private readonly DBInteractor db;
        private readonly FlightPlanManager manager;

        public FlightPlanController(DBInteractor newDB)
        {
            db = newDB;
            manager = new FlightPlanManager(db);

        }

        // GET: api/FlightPlan/5
        //return flight plan by id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFlightPlan(string id)
        {
            //FlightPlan flightPlan = null;
            // Search in local flight plans.
            var flightPlan = await db.FlightPlans.FindAsync(id);
            if (flightPlan == null)
            {
                // If not found, search for the id in external flights db.
                var extFlightPlan = await db.ExternalFlights.FindAsync(id);
                if (extFlightPlan == null)
                {
                    return NotFound();
                }
                // Build request string.    
                string request = "/api/FlightPlan/" + id;
                string serverUrl = extFlightPlan.ExternalServerUrl;
                request = serverUrl + request;
                // Send the request and get FlightPlan object.
                var response = await ServerManager.MakeRequest(request);
                try
                {
                    flightPlan = JsonConvert.DeserializeObject<FlightPlan>(response);
                    if (flightPlan.InitialLocation == null ||
                        flightPlan.Segments == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "one of the fields is null, try again");
                    }
                }
                catch (JsonException je)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, je.Data);
                }
            }
            else
            {
                //get all the details about the internal flight
                var flightSegments = db.Segments.Where(s => s.FlightId == flightPlan.Id).OrderBy(s => s.SegmentNumber).ToList();
                var flightinitLocation = db.InitLocations.Where(i => i.FlightId == flightPlan.Id).First();
                DateTime UtcTime = (TimeZoneInfo.ConvertTimeToUtc(flightinitLocation.DateTime));
                UtcTime.ToString("yyyy-MM-dd-THH:mm:ssZ");
                flightinitLocation.DateTime = UtcTime;
                flightPlan.Segments = flightSegments;
                flightPlan.InitialLocation = flightinitLocation;
            }
            string output = JsonConvert.SerializeObject(flightPlan);
            return Ok(output);
        }



        // POST: api/FlightPlan
        // insert new flight plan
        [HttpPost]
        public ActionResult<FlightPlan> PostFlightPlan([FromBody] FlightPlan flightPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var flight = manager.CreateNewFlightPlan(flightPlan);
            return CreatedAtAction("GetFlightPlan", new { id = flight.Id }, flight);
        }

    }
}
