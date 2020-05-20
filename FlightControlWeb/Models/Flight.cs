using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Flight
    {
        public long FlightId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Passengers { get; set; }
        public string CompanyName { get; set; }
        public DateTime Date { get; set; }
        public bool IsExternal { get; set; }

    }
}
