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
        public IEnumerable<FlightPlan> GetAllFlights();
        public IEnumerable<FlightPlan> GetInternalFlights();
        public long GanerateID();


    }

}
