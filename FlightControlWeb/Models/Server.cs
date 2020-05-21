using System.Text.Json.Serialization;


namespace FlightControlWeb.Models
{
    public class Server
    {
        public long Id { get; set; }

        [JsonPropertyName("ServerURL")]
        public string Url { get; set; }
    }
}
