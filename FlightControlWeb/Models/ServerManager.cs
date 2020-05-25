using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace FlightControlWeb.Models
{
    internal class ServerManager
    {
        private readonly DBInteractor db;

        internal ServerManager(DBInteractor db)
        {
            this.db = db;
        }

        internal static async Task<string> MakeRequest(string uri)
        {
            var client = new HttpClient();
            string jsonString = await client.GetStringAsync(uri);
            /*dynamic result = JsonConvert.DeserializeObject<dynamic>(jsonString);*/
            return jsonString;
        }

        internal List<Server> GetServers(string servId)
        {
            return db.Servers.Where(server => server.Id == servId).ToList();
        }
    }
}
