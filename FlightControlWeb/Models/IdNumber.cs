using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace FlightControlWeb.Models
{
    public class IdNumber
    {

        public IdNumber(long id)
        {
            Id = id;
        }

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
