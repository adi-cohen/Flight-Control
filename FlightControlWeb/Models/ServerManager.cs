using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightControlWeb.Models
{
    public class ServerManager
    {
        public static async Task<dynamic> makeExternalRequest(string uri)
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync(uri);
            dynamic json = JsonConvert.DeserializeObject(result);
            return json;
        }
    }
}
