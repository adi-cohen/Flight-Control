using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestAPI.API.Request;

namespace TestAPI.APITests
{
    public class Tests
    {
        [Category("Test")]
        [Test]
        public async Task TestAPICall()
        {
            TestAPI.API.Request.API api = new TestAPI.API.Request.API();
            var response = await api.testRequest();
        }
    }
}
