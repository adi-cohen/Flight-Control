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
using Microsoft.VisualBasic.CompilerServices;
using System.Text.RegularExpressions;
using System.Net;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly DBInteractor db;
        private FlightManager manager;

        public FlightsController(DBInteractor context)
        {
            db = context;
            manager = new FlightManager(new FlightPlanManager(context), context);
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
                //ask other servers
                
            }
            List<Flight> internalFlights =  manager.getAllFlights(UtcTime);
            flightList.AddRange(internalFlights);

            string output = JsonConvert.SerializeObject(flightList);
            return output;

        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HttpStatusCode>> DeleteFlight(int id)
        {
            long? deletedId =  manager.RemoveFlight(id);
            if (deletedId == null)
            {
                return NotFound();
            }
            return HttpStatusCode.NoContent; 
        }

    }
}
