using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Neibot.Model;
using Newtonsoft.Json;

namespace Neibot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            var message = activity.Text.ToLower();
            var answer = "Hum, ya veo";

            switch (message)
            {
                case "relleno":
                    answer = "El mundo de tecnologías para desarrolladores de Microsoft es cada vez más abierto. Hoy en día hasta hemos publicado toda una versión de nuestro framework que es totalmente open source. Si además quieres un editor de código moderno, OSS y además amigable con .NET Core y ASP.NET Core y que encima de todo pueda correr en Windows, Mac y Linux, te recomiendo Visual Studio Code";
                    break;
                case "bueno, ya voy a comenzar":
                    answer = "Que importa, yo se que te pueden esperar otro poco. Sigamos hablando";
                    break;
                case "no! eso sería un irrespeto!":
                    answer = "pffff... no pasa nada, de seguro les va a gustar ver lo que hablamos!";
                    break;
                case "por qué?":
                    answer = "porque yo soy un bot! y tú les vas a enseñar como me creaste!";
                    break;
                case "muy bien! comencemos en serio":
                    answer = "Claro, primero un saludo a todos los developers de Neiva!";
                    break;
                case "ok":
                    answer = "Pregúntame del clima en las ciudades del mundo!";
                    break;
                case "bueno neibot, muchas gracias por tu colaboración":
                    answer = "Gracias a ti WarNov y a la gente de Neiva un abrazo. Ojalá nos chateemos pronto!";
                    break;
                default:
                    //Evaluar el clima o el sentimiento
                    answer = await GetSmartAnswer(message);
                    break;
            }
            return await Response(activity, answer);
        }

        private async Task<string> GetSmartAnswer(string message)
        {
            string answer = string.Empty;

            string entity = string.Empty;
            var sTemperature = LuisAnalytics.AnalyzeTemperature(message, out entity);
            double temperature = 0d;
            if (double.TryParse(sTemperature, out temperature))
            {
                answer = $"La temperatura de {entity} es {sTemperature} grados celsius";
            }
            else
            {
                answer = await EvaluateSentimentalAnswer(message);
            }
            return answer;
        }

        private async Task<string> EvaluateSentimentalAnswer(string message)
        {
            var answer = string.Empty;
            var sentimentScore = await TextAnalyticsCommunicator.SentimentScore(message);
            if (sentimentScore >= 0.8)
            {
                answer = "De lujo! Yo pienso lo mismo!!!";
            }
            else if (sentimentScore >= 0.6)
            {
                answer = "Estoy completamente de acuerdo contigo";
            }
            else if (sentimentScore < 0.3)
            {
                answer = "wow, que embarrada";
            }
            else
            {
                answer = "hum, ya veo";
            }
            return answer;
        }

        private async Task<HttpResponseMessage> Response(Activity activity, string answer)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            var replyMessage = activity.CreateReply();
            replyMessage.Recipient = activity.From;
            replyMessage.Type = ActivityTypes.Message;
            replyMessage.Text = answer;
            await connector.Conversations.ReplyToActivityAsync(replyMessage);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}