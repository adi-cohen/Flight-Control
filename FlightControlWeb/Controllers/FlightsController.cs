using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.VisualBasic.CompilerServices;
using System.Text.RegularExpressions;
using System.Net;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly ServerManager _servManager;
        private readonly DBInteractor db;
        private FlightManager manager;

        public FlightsController(DBInteractor context)
        {
            db = context;
            manager = new FlightManager(new FlightPlanManager(context), context);
            _servManager = new ServerManager(context);
        }

        

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
                List<Flight> externalFlights = new List<Flight>();

                // Build the request string to send.
                string request = "/api/Flights?relative_to=";
                request += relative_to;

                // Pass the HTTP request to all registered external servers.
                foreach (Server serv in db.Servers)
                {
                    /*string serverUrl = serv.Url;
                     request = serverUrl + request;
                     // Send the request and get Flight object.
                     Flight response = await ServerManager.makeRequest(request);
                     externalFlights.Add(response);
                     // Add to flightId -> URL mapping.
                     ExternalFlight newExtFlight = new ExternalFlight();
                     newExtFlight.FlightId = response.FlightId;
                     newExtFlight.ExternalServerUrl = serv.Url;
                     db.ExternalFlights.Add(newExtFlight);*/

                    // dont forget add the flights to flightList
                }
            }
            List<Flight> internalFlights = manager.getAllFlights(UtcTime);
            flightList.AddRange(internalFlights);

            string output = JsonConvert.SerializeObject(flightList);
            return output;

        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HttpStatusCode>> DeleteFlight(string id)
        {
            string deletedId =  manager.RemoveFlight(id);
            if (deletedId == null)
            {
                return NotFound();
            }
            return HttpStatusCode.NoContent; 
        }

    }
}
