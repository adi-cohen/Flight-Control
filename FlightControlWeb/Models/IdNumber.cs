using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace FlightControlWeb.Models
{
    public class IdNumber
    {

        public IdNumber(string id)
        {
            Id = id;
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
