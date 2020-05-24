using System.Text.Json.Serialization;


namespace FlightControlWeb.Models
{
    public class Server
    {
        [JsonPropertyName("ServerId")]
        public string Id { get; set; }

        [JsonPropertyName("ServerURL")]
        public string Url { get; set; }
    }
}
