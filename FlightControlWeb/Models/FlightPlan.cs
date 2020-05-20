using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlan
    {
        public long Id { get; set; }
        [JsonPropertyName("passengers")]
        public int Passengers { get; set; }
        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }


        [NotMapped]
        [JsonPropertyName("initial_location")]
        public InitialLocation InitialLocation { get; set; }

        [JsonPropertyName("segments")]
        public List<Segment> Segments { get; set; }

       
    }

}
