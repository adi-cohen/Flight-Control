using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlan
    {
        public long Id { get; set; }
        public int Passengers { get; set; }
        public string CompanyName { get; set; }
        public double StartLongitude { get; set; }
        public double StartLatitude { get; set; }
        public DateTime StartDate { get; set; }
        public List<Segment> Segments { get; set; }
        
    }

}
