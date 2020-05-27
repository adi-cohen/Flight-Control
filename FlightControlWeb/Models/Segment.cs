using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models
{
    public class Segment
    {
        [IgnoreDataMember]
        public string Id { get; set; }

        [ForeignKey("FlightPlan")]
        [IgnoreDataMember]

        public string FlightId { get; set; }

        [IgnoreDataMember]

        public long SegmentNumber { get; set; }

        [JsonProperty(PropertyName = "longitude")]

        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]

        public double Latitude { get; set; }
        [JsonPropertyName("timespan_seconds")]

        [JsonProperty(PropertyName = "timespan_seconds")]
        public int TimeInSeconds { get; set; }
    }
}
