using FlightControlWeb.Controllers;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlightControlTest
{
    internal class StubIServerManager : IServerManager
    {
        public StubIServerManager()
        {
        }

        public Task<string> MakeRequest(string uri)
        {

            string V = "{\"passengers\":1,\"company_name\":\"testCompany\",\"initial_location\":{\"longitude\":25,\"latitude\":20,\"date_time\":\"2020-05-26T11:29:26Z\"},\"segments\":[{\"longitude\":30,\"latitude\":40,\"timespan_seconds\":90000},{\"longitude\":59,\"latitude\":50,\"timespan_seconds\":95600},{\"longitude\":70,\"latitude\":65,\"timespan_seconds\":9500}]}";

            return Task.FromResult<string>(V);
        }
    }
}