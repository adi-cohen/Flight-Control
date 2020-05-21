using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightControlWeb.Models;
using Newtonsoft.Json;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private readonly DBInteractor db;
        private readonly IFlightPlanManager manager;
        public FlightPlanController(DBInteractor newDb)
        {
            db = newDb;
            manager = new FlightPlanManager(db);
        }

        // GET: api/FlightPlan/5
        //return flight plan by id
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetFlightPlan(long id)
        {
            FlightPlan flightPlan = await db.FlightPlans.FindAsync(id);
            if (flightPlan == null)
            {
                //fing the flight in external servers
            }
            //if the flight is not external and internal 
            if (flightPlan == null)
            {
                return NotFound();
            }
            List<Segment> flightSegments = db.Segments.Where(s => s.FlightId == flightPlan.Id).ToList();
            InitialLocation flightinitLocation = db.InitLocations.Where(i => i.FlightId == flightPlan.Id).First();
            DateTime UtcTime = (TimeZoneInfo.ConvertTimeToUtc(flightinitLocation.DateTime));
            UtcTime.ToString("yyyy-MM-dd-THH:mm:ssZ");
            flightinitLocation.DateTime = UtcTime;
            flightPlan.Segments = flightSegments;
            flightPlan.InitialLocation = flightinitLocation;
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
