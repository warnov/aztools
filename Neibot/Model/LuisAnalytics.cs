using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace Neibot.Model
{

    public class Resolution
    {
    }

    public class Value
    {
        public string entity { get; set; }
        public string type { get; set; }
        public Resolution resolution { get; set; }
    }

    public class Parameter
    {
        public string name { get; set; }
        public bool required { get; set; }
        public List<Value> value { get; set; }
    }

    public class Action
    {
        public bool triggered { get; set; }
        public string name { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class TopScoringIntent
    {
        public string intent { get; set; }
        public double score { get; set; }
        public List<Action> actions { get; set; }
    }

    public class Resolution2
    {
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public double score { get; set; }
        public Resolution2 resolution { get; set; }
    }

    public class Dialog
    {
        public string contextId { get; set; }
        public string status { get; set; }
    }

    public class LuisAnalytics
    {
        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
        public List<Entity> entities { get; set; }
        public Dialog dialog { get; set; }

        public static string AnalyzeTemperature(string message, out string entity)
        {
            var ret = "No encontrado";
            using (WebClient wc = new WebClient())
            {
                var jsonLuisString = wc.DownloadString($"https://api.projectoxford.ai/luis/v1/application/preview?id=cea3dd56-e7f2-4e8f-ab7d-0dd56c699ce2&subscription-key=cb2bfa0cce4943ee8584c18c6ca3ad45&q={message}");
                var jsonLuis = JsonConvert.DeserializeObject<LuisAnalytics>(jsonLuisString);
                var intent = jsonLuis.topScoringIntent.intent;
                entity = jsonLuis.entities.Count > 0 ? jsonLuis?.entities[0].entity : null;
                if (intent == "ObtenerClima" && entity != null)
                {
                    var temperature = WeatherAnalytics.GetTemperature(entity);
                    ret = temperature != 0 ? temperature.ToString() : ret;
                }
            }
            return ret;
        }
    }
}