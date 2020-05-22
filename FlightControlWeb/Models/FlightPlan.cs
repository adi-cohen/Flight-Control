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
    public class FlightPlan
    {
        [IgnoreDataMember]
        [JsonProperty(PropertyName = "flight_id")]
        public long Id { get; set; }

        [JsonPropertyName("passengers")]
        [JsonProperty(PropertyName = "passengers")]
        public int Passengers { get; set; }

        [JsonPropertyName("company_name")]
        [JsonProperty(PropertyName = "company_name")]
        public string CompanyName { get; set; }

        [NotMapped]
        [JsonPropertyName("initial_location")]
        [JsonProperty(PropertyName = "initial_location")]
        public InitialLocation InitialLocation { get; set; }

        [JsonPropertyName("segments")]
        [JsonProperty(PropertyName = "segments")]
        public List<Segment> Segments { get; set; }

       
    }

}
