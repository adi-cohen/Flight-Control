using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestAPI.API.Request
{
    class API
    {
        private HttpClient restClient = new HttpClient();
        private string URI = "https://api.publicapis.org";

        public async Task<string> testRequest()
        {
            var Builder = new System.UriBuilder($"{URI}/api/getFlight{1}");
            var response = await restClient.GetAsync(Builder.Uri);
            var context = await response.Content.ReadAsStringAsync();
            return context;
        }
    }
}
