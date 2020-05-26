using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightControlTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //create In Memory Database
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
                context.FlightPlans.Add(new FlightPlan
                {
                    Passengers = 1,
                    CompanyName = "companyTest",
                    InitialLocation = new InitialLocation
                    {

                    }
                })
               
                context.SaveChanges();
            }

            //act
            using (var context = new DBInteractor(options))
            {

            }


                //arrange 
                var flightController = new FlightsController();
            var flights = flightController.GetFlights("");
            Assert.AreEqual(flights.Result, null); // Why 123?

            
        }


    }
}
