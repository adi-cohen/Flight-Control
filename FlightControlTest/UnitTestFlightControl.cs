using FakeItEasy;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Threading.Tasks;


namespace FlightControlTest
{
    [TestClass]
    public class UnitTestFlightControl
    {
        [TestMethod]
        public async Task GetFlightPlan_ShouldReturnFlightPlanById_fromExternalServer()
        {

            //arrange - create In Memory Database
            var options = new DbContextOptionsBuilder<DBInteractor>()
            .UseInMemoryDatabase(databaseName: "filghtContolDB")
            .Options;

            //// Create mocked Context by seeding Data as per Schema///
            using (var context = new DBInteractor(options))
            {
                context.Servers.Add(new Server
                {
                    Id = "1",
                    Url = "serverTest"
                });
                context.ExternalFlights.Add(new ExternalFlight
                {
                    FlightId = "1",
                    ExternalServerUrl = "serverTest"
                });

                context.SaveChanges();
            }

            //act
            using (var context = new DBInteractor(options))
            {

                var stub = new StubIServerManager();

                FlightPlanController controller = new FlightPlanController(context)
                {
                    ServerManagerProp = stub
                };
                var actionResult = await controller.GetFlightPlan("1");
                var okObject = actionResult as OkObjectResult;
                //var actualResult = result.Value;
                var flight = JsonConvert.DeserializeObject<FlightPlan>(okObject.Value.ToString());
                Assert.AreEqual(1, flight.Passengers);
                Assert.AreEqual("testCompany", flight.CompanyName);
                Assert.AreEqual(3, flight.Segments.Count);
                Assert.AreEqual(20, flight.InitialLocation.Latitude);
                Assert.AreEqual(25, flight.InitialLocation.Longitude);
            }
        }


    }
}
