using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class InitialLocation
    {
        [IgnoreDataMember]
        public string Id { get; set; }

        [ForeignKey("FlightPlan")]
        [IgnoreDataMember]
        public string FlightId { get; set; }

        [JsonPropertyName("longitude")]
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("latitude")]
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("date_time")]
        [JsonProperty(PropertyName = "date_time")]
        public DateTime DateTime { get; set; }

    }
}
