using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    interface IFlightPlanManager
    {
        public void DeleteFlightPlan(int id);
        public FlightPlan GetFlightPlan(int id);
        public void AddFlightPlan(FlightPlan flightplan);
        public List<FlightPlan> GetActiveFlights(DateTime time);
        public long GanerateID();

        public List<Segment> GetFlightPlanSegments(long id);


        


    }

}
