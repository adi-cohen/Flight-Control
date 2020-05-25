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
        private readonly DBInteractor db;

        public ServerManager(DBInteractor db)
        {
            this.db = db;
        }

        public static async Task<string> MakeRequest(string uri)
        {
            var client = new HttpClient();
            string jsonString = await client.GetStringAsync(uri);
            /*dynamic result = JsonConvert.DeserializeObject<dynamic>(jsonString);*/
            return jsonString;
        }

        public List<Server> GetServers(string servId)
        {
            return db.Servers.Where(s => s.Id == servId).ToList();
        }
    }
}
