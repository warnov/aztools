using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neibot.Model;
using Newtonsoft.Json;

namespace Neibot.Controllers
{
    public class LuisController : ApiController
    {
        // GET: api/Luis/Cual%20es%20la%20temperatura%20de%20bogota
        public string Get(string id)
        {
            string entity = string.Empty;
            return LuisAnalytics.AnalyzeTemperature(id, out entity);
        }
    }
}
