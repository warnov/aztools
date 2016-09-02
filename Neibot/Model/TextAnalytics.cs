
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class BatchInput
{
    public List<DocumentInput> documents { get; set; }
}
public class DocumentInput
{
    public double id { get; set; }
    public string text { get; set; }
}
// Classes to store the result from the sentiment analysis
public class BatchResult
{
    public List<DocumentResult> documents { get; set; }
}
public class DocumentResult
{
    public double score { get; set; }
    public string id { get; set; }
}

public static class TextAnalyticsCommunicator
{
    public static async Task<double> SentimentScore(string message)
    {
        var ret = 0d;

        const string apiKey = "d8c5644161084c948780ee1d69338f6f";
        string queryUri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";

        using (HttpClient client = new HttpClient())
        {            
            //Ajustamos el cliente para llamar a cognitive services
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            //Ajustamos el mensaje:
            BatchInput sentimentInput = new BatchInput();
            sentimentInput.documents = new List<DocumentInput>();
            sentimentInput.documents.Add(new DocumentInput()
            {
                id = 1,
                text = message
            });

            //Serializamos el mensaje y lo enviamos usando el cliente, a través de un post
            //Primero a JSON
            var sentimentJsonInput = JsonConvert.SerializeObject(sentimentInput);
            //De JSON a bytes
            byte[] byteData = Encoding.UTF8.GetBytes(sentimentJsonInput);
            //Creamos contenido
            var content = new ByteArrayContent(byteData);
            //Le añadimos el header
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //Enviamos post y obtenemos resultado
            var responseBytesFromAnalytics = await client.PostAsync(queryUri, content);
            var responseJsonString = await responseBytesFromAnalytics.Content.ReadAsStringAsync();
            var responseBatchResultObject = JsonConvert.DeserializeObject<BatchResult>(responseJsonString);
            ret = responseBatchResultObject.documents[0].score;
        }
        return ret;
    }
}
