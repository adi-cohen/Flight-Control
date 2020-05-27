using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    internal class Flight
    {
        [JsonProperty(PropertyName = "flight_id")]
        internal string FlightId { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        internal double Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        internal double Latitude { get; set; }

        [JsonProperty(PropertyName = "passengers")]
        internal int Passengers { get; set; }

        [JsonProperty(PropertyName = "company_name")]
        internal string CompanyName { get; set; }

        [JsonProperty(PropertyName = "date_time")]
        internal DateTime Date { get; set; }

        [JsonProperty(PropertyName = "is_external")]
        internal bool IsExternal { get; set; }

    }
}
