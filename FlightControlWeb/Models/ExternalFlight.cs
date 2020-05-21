using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models
{
    public class ExternalFlight
    {
        [Key]
        [JsonPropertyName("flight_id")]
        public long FlightId { get; set; }

        [JsonPropertyName("external_url")]
        public string ExternalServerUrl { get; set; }

    }
}
