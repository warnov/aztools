using System.Web.Http;
using Neibot.Model;


namespace Neibot.Controllers
{
    public class WeatherController : ApiController
    {
        // GET: api/Weather/bogota
        public string Get(string id)
        {
            var temperature = WeatherAnalytics.GetTemperature(id);
            return temperature == 0 ? "No encontrado" : temperature.ToString();
        }
    }
}
