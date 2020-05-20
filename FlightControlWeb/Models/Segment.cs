﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Segment
    {
        public long Id { get; set; }

        [ForeignKey("FlightPlan")]
        public long FlightId { get; set; }
        public long SegmentNumber { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [JsonPropertyName("timespan_seconds")]

        public int TimeInSeconds { get; set; }
    }
}
