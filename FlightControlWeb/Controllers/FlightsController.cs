using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightControlWeb.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly DBInteractor _context;
        private readonly ServerManager _servManager;
        private FlightManager manager;

        public FlightsController(DBInteractor context, ServerManager servManager)
        {
            _context = context;
            _servManager = servManager;
            manager = new FlightManager(new FlightPlanManager(context), context);
        }

       /* // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlight()
        {
            return await _context.Flight.ToListAsync();
        }*/

        // GET: api/Flights/
        [HttpGet("")]
        public async Task<string> GetFlights([FromQuery]string relative_to, [FromQuery] string sync)//, string sync = null)
        {
            bool ToSyncAll = Request.Query.ContainsKey("sync_all");
            DateTime UtcTime = (TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(relative_to)));
            //UtcTime.ToString("yyyy-MM-dd-THH:mm:ssZ");
            List<Flight> flightList = new List<Flight>();
            if (ToSyncAll)
            {
                // list to add Flight Objects

                // Build the request string to send. 
                string request = "/api/Flights?relative_to=";
                request += relative_to;

                // Pass the HTTP request to all registered external servers.
                foreach (Server serv in _context.Servers)
                {
                    string serverUrl = serv.Url;
                    request = serverUrl + request;
                    // Send the request.
                    dynamic response = await ServerManager.makeRequest(request);
                }
            }
            List<Flight> internalFlights =  manager.getAllFlights(UtcTime);
            flightList.AddRange(internalFlights);

            string output = JsonConvert.SerializeObject(flightList);
            return output;

        }

     

        // POST: api/Flights
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
            _context.Flight.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.FlightId }, flight);
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Flight>> DeleteFlight(int id)
        {
            var flight = await _context.Flight.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.Flight.Remove(flight);
            await _context.SaveChangesAsync();

            return flight;
        }

        private bool FlightExists(int id)
        {
            return _context.Flight.Any(e => e.FlightId == id);
        }
    }
}
