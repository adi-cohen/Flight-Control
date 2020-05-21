using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightControlWeb.Models;

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


        /*// GET: api/FlightPlan
        // return all flights plan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightPlan>>> GetFlightPlans()
        {
            return await db.FlightPlans.ToListAsync();
        }*/



        // GET: api/FlightPlan/5
        //return flight plan by id
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightPlan>> GetFlightPlan(long id)
        {
            var flightPlan = await db.FlightPlans.FindAsync(id);

            if (flightPlan == null)
            {
                return NotFound();
            }
            return flightPlan;
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
            //generate random id
            long FlightId = manager.GanerateID();
            flightPlan.Id = FlightId;
            db.FlightPlans.Add(flightPlan);
            
            //adding all segments to DB
            int segmentNum = 1;
            foreach (Segment element in flightPlan.Segments)
            {
                //adding segment number for segments in the same flight plan
                element.SegmentNumber = segmentNum;
                segmentNum ++;
                element.FlightId = FlightId;
                element.Id = manager.GanerateID();
                db.Segments.Add(element);
            }

            //adding InitialLocation to DB
            flightPlan.InitialLocation.FlightId = FlightId;
            flightPlan.InitialLocation.Id = manager.GanerateID();
            db.InitLocations.Add(flightPlan.InitialLocation);

            await db.SaveChangesAsync();
            return CreatedAtAction("GetFlightPlan", new { id = flightPlan.Id }, flightPlan);
        }

        // DELETE: api/FlightPlan/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FlightPlan>> DeleteFlightPlan(long id)
        {
            var flightPlan = await db.FlightPlans.FindAsync(id);
            if (flightPlan == null)
            {
                return NotFound();
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
            await db.SaveChangesAsync();
            return flightPlan;
        }

        
    }
}
