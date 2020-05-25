using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
            if (generator.IsUnique(new IdNumber(serv.Id)))
            {
                db.Servers.Add(serv);
                await db.SaveChangesAsync();
                return HttpStatusCode.Created;
            }
            return HttpStatusCode.BadRequest;
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
