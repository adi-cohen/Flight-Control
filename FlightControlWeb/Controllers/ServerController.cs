using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightControlWeb.Controllers
{
    [Route("api/servers")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly DBInteractor db;

        public ServerController(DBInteractor newDB)
        {
            db = newDB;
        }

        // GET: api/Server
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Server>>> GetServers()
        {
            return await db.Servers.ToListAsync();
        }

        // POST: api/Server
        [HttpPost]
        public async Task<ActionResult<HttpStatusCode>> PostServer([FromBody] Server serv)
        {
            IdGenerator generator = new IdGenerator(db);
            IdNumber temp = new IdNumber(serv.Id);

            // Check if provided ID or URL already exist in DB.
            var val = db.Servers.Where(s => s.Url == serv.Url).FirstOrDefault();
            if (generator.IsUnique(temp) && val == null)
            {
                // If not, add new server.
                db.Servers.Add(serv);
                db.IdNumbers.Add(temp);
                await db.SaveChangesAsync();
                return HttpStatusCode.Created;
            }
            // Otherwise return conflict status code.
            return StatusCode(409);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Server>> DeleteServer(string id)
        {
            var serv = await db.Servers.FindAsync(id);
            if (serv == null)
            {
                return NotFound();
            }
            db.Servers.Remove(serv);
            await db.SaveChangesAsync();
            return serv;
        }
    }
}
