using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class InitialLocation
    {
        public long Id { get; set; }

        [ForeignKey("FlightPlan")]
        public long FlightId { get; set; }
        [JsonPropertyName("longitude")]

        
        public double Longitude { get; set; }
        [JsonPropertyName("latitude")]

        public double Latitude { get; set; }
        [JsonPropertyName("date_time")]

        public DateTime DateTime { get; set; }

    }
}
