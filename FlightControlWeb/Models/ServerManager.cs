using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using FlightControlWeb.Controllers;

namespace FlightControlWeb.Models
{
    public class ServerManager : IServerManager
    {
        private readonly DBInteractor db;

        internal ServerManager(DBInteractor db)
        {
            this.db = db;
        }

        /*internal async Task<string> MakeRequest(string uri)
        {
            var client = new HttpClient();
            string jsonString = await client.GetStringAsync(uri);
            return jsonString;
        }*/

        internal List<Server> GetServers(string servId)
        {
            return db.Servers.Where(server => server.Id == servId).ToList();
        }

        async Task<string> IServerManager.MakeRequest(string uri)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return jsonString;
            }
            else
            {
                return null;
            }
        }
    }
}
