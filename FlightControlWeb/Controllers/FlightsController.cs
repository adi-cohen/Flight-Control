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
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly DBInteractor db;
        private readonly FlightManager manager;

        public FlightsController(DBInteractor context)
        {
            db = context;
            manager = new FlightManager(new FlightPlanManager(context), context);
        }



        // GET: api/Flights/
        [HttpGet("")]
        public async Task<ActionResult> GetFlights([FromQuery]string relative_to)
        {
            bool ToSyncAll = Request.Query.ContainsKey("sync_all");
            DateTime UtcTime = (TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(relative_to)));
            var flightList = new List<Flight>();
            if (ToSyncAll)
            {
                var externalFlights = new List<Flight>();
                // Build the request string to send.
                string requestParams = "/api/Flights?relative_to=" + relative_to;
                // Pass the HTTP request to all registered external servers.
                foreach (Server serv in db.Servers)
                {
                    var flightsFromCurrServ = new List<Flight>();
                    string requestFull = serv.Url + requestParams;
                    // Send the request and get Flight object.
                    var response = await ServerManager.MakeRequest(requestFull);
                    // Desirialize the list of JSON object we got into list of Flights.
                    try
                    {
                        var tmp = JsonConvert.DeserializeObject<List<Flight>>(response);
                        flightsFromCurrServ.AddRange(tmp);
                        // Add to flightId -> URL mapping db.
                        foreach (Flight f in flightsFromCurrServ)
                        {
                            if (f.FlightId == null)
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError);
                            }
                            f.IsExternal = true;
                            var temp = db.ExternalFlights.Find(f.FlightId);
                            if (temp == null)
                            {
                                ExternalFlight newExtFlight = new ExternalFlight();
                                newExtFlight.FlightId = f.FlightId;
                                newExtFlight.ExternalServerUrl = serv.Url;
                                db.ExternalFlights.Add(newExtFlight);
                                db.SaveChanges();
                            }
                        }
                        externalFlights.AddRange(flightsFromCurrServ);
                    }
                    catch (JsonException je)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, je);
                    }

                }
                // Add the external flights to general flightList.
                flightList.AddRange(externalFlights);
            }
            var internalFlights = manager.GetAllFlights(UtcTime);
            flightList.AddRange(internalFlights);

            string output = JsonConvert.SerializeObject(flightList);
            return Ok(output);
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public ActionResult<HttpStatusCode> DeleteFlight(string id)
        {
            string deletedID = manager.RemoveFlight(id);
            if (deletedID == null)
            {
                return NotFound();
            }
            return HttpStatusCode.NoContent;
        }

    }
}
