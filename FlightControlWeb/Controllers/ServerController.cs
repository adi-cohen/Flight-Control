using System.Collections.Generic;
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
        private readonly ServerManager manager;

        public ServerController(DBInteractor newDb, ServerManager mngr)
        {
            db = newDb;
            manager = mngr;
        }

        // GET: api/Server
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Server>>> GetServers()
        {
            return await db.Servers.ToListAsync();
        }

        // POST: api/Server
        [HttpPost]
        public async void PostServer([FromBody] Server serv)
        {
            serv.Id = manager.GanerateID();
            db.Servers.Add(serv);
            await db.SaveChangesAsync();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Server>> DeleteServer(long id)
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
