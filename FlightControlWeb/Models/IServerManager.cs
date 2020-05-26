using System.Threading.Tasks;

namespace FlightControlWeb.Controllers
{
    public interface IServerManager
    {
        public Task<string> MakeRequest(string uri);
    }
}