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

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private readonly DBInteractor db;
        private readonly IFlightPlanManager manager;
        private readonly ServerManager servManager;
        private readonly IdGenerator generator;
        public FlightPlanController(DBInteractor newDb)
        {
            db = newDb;
            manager = new FlightPlanManager(db);
            servManager = new ServerManager(db);
            generator = new IdGenerator(db);

        }

        // GET: api/FlightPlan/5
        //return flight plan by id
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetFlightPlan(string id)
        {
            //FlightPlan flightPlan = null;
            // Search in local flight plans.
            FlightPlan flightPlan = await db.FlightPlans.FindAsync(id);
            if (flightPlan == null)
            {
                // If not found, search for the id in external flights db.
                ExternalFlight extFlightPlan = await db.ExternalFlights.FindAsync(id);
                if (extFlightPlan == null)
                {
                    return NotFound();
                }
                // Build request string.
                string request = "/api/FlightPlan/" + id;
                string serverUrl = extFlightPlan.ExternalServerUrl;
                request = serverUrl + request;
                // Send the request and get FlightPlan object.
                var response = await ServerManager.makeRequest(request);
                flightPlan = JsonConvert.DeserializeObject<FlightPlan>(response);
            }
            else
            {
                //get all the details about the internal flight
                List<Segment> flightSegments = db.Segments.Where(s => s.FlightId == flightPlan.Id).OrderBy(s => s.SegmentNumber).ToList();
                InitialLocation flightinitLocation = db.InitLocations.Where(i => i.FlightId == flightPlan.Id).First();
                DateTime UtcTime = (TimeZoneInfo.ConvertTimeToUtc(flightinitLocation.DateTime));
                UtcTime.ToString("yyyy-MM-dd-THH:mm:ssZ");
                flightinitLocation.DateTime = UtcTime;
                flightPlan.Segments = flightSegments;
                flightPlan.InitialLocation = flightinitLocation;
            }
            string output = JsonConvert.SerializeObject(flightPlan);
            return output;
        }

        

        // POST: api/FlightPlan
        // insert new flight plan
        [HttpPost]
        public async Task<ActionResult<FlightPlan>> PostFlightPlan([FromBody] FlightPlan flightPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            FlightPlan flight =  manager.createNewFlightPlan(flightPlan);
            return CreatedAtAction("GetFlightPlan", new { id = flight.Id }, flight);
        }

   
    }
}
