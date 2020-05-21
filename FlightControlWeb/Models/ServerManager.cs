using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace FlightControlWeb.Models
{
    public class ServerManager
    {
        private readonly DBInteractor _db;

        public ServerManager(DBInteractor db)
        {
            _db = db;
        }

        public static async Task<dynamic> makeRequest(string uri)
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync(uri);
            dynamic json = JsonConvert.DeserializeObject(result);
            return json;
        }

        public List<Server> GetServers(long servId)
        {
            return _db.Servers.Where(s => s.Id == servId).ToList();
        }

        public long GanerateID()
        {
            Random rand = new Random();
            long id = rand.Next(10000, 999999999);
            return id;
        }
    }
}
