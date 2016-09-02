using System;
using System.Net.Http;
using System.Web.Http;
using Neibot.Model;
using Newtonsoft.Json;

namespace Neibot.Controllers
{
    public class WeatherController : ApiController
    {
        // GET: api/Weather/bogota
        public string Get(string id)
        {
            var response = "No encontrado";
            using (var hc = new HttpClient())
            {
                hc.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/weather");
                var jsonWeatherString = hc.GetStringAsync($"?q={id}&APPID=b45f3fb5f8b4200062d1bbb572ffed97").Result;
                var jsonWeather = JsonConvert.DeserializeObject<WeatherWrapper>(jsonWeatherString);
                var kelvin = jsonWeather?.main?.temp ?? 0;
                if (kelvin != 0)
                {
                    var celsius = kelvin - 273.15;
                    response = celsius.ToString();
                }
            } 
            return response;
        }
    }
}
