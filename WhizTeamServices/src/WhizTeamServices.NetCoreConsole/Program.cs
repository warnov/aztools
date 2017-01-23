using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WhizTeamServices.NetCoreConsole.RestAPIModels;
using static WhizTeamServices.NetCoreConsole.ConfigHelper;

namespace WhizTeamServices.NetCoreConsole
{
    public class Program
    {
        static IConfigurationSection domainsSection = Config.GetSection("domains");
        public static void Main(string[] args)
        {
            string workingDomain, workingProject, description, sourceControlProvider, templateId;
            if (args.Length > 0)
            {
                workingDomain = args[0];
                workingProject = args[1];
                description = args[2];
                var defaults = Config.GetSection("defaults");
                sourceControlProvider = defaults["sourceControlProvider"];
                templateId = defaults["template"];
            }
            else
            {
                //Domain
                workingDomain = GetDomain();

                //Project Name
                Console.WriteLine("Project Name: ");
                workingProject = Console.ReadLine();

                //Project Description
                Console.WriteLine("Project Description: ");
                description = Console.ReadLine();

                //SOURCE CONTROL           
                sourceControlProvider = GetSourceControlProvider();

                //TEMPLATE ID
                templateId = GetTemplateId();                
            }

            //PAT
            var pat = domainsSection[workingDomain];

            //Message Body
            var messageBody = GetMessageBody(workingProject, description, sourceControlProvider, templateId);

            //PostExecution
            var postResult = ExecutePost(workingDomain, pat, messageBody);

            Console.WriteLine($"{postResult}\n\nPress Enter to Finish...");

            Console.ReadLine();
        }


        private static string ExecutePost(string workingDomain, string pat, StringContent messageBody)
        {
            string responseBody=string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string _credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", pat)));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);
                var url = Config["apiPath"];

                var request = new HttpRequestMessage(HttpMethod.Post,
                    string.Format(url, workingDomain))
                {
                    Content = messageBody
                };              
                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    var code = response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = $"{code}: {response.Content.ReadAsStringAsync().Result}";
                    }                  
                                      
                }
                catch (Exception exc)
                {
                    responseBody = $"ERROR: {exc.Message}. {exc.InnerException?.Message}";
                }
            }
            return responseBody;
        }
        private static StringContent GetMessageBody(string workingProject, string description, string sourceControlProvider, string templateId)
        {
            ProjectCreator body = new ProjectCreator(workingProject, description, sourceControlProvider, templateId);
            var jsonBody = JsonConvert.SerializeObject(body);
            return new StringContent(jsonBody, Encoding.UTF8, "application/json");
        }

        private static string GetTemplateId()
        {
            var templatesSection = Config.GetSection("templates");
            Console.WriteLine("Available Templates: ");
            foreach (var provider in templatesSection.GetChildren())
            {
                Console.WriteLine(provider.Key);
            }
            Console.WriteLine("Template: ");
            var templateKey = Console.ReadLine();
            return templatesSection[templateKey];
        }

        private static string GetSourceControlProvider()
        {
            var sourceControlProviders = Config.GetSection("sourceControl").GetChildren();
            Console.WriteLine("Available Source Control Providers: ");
            foreach (var provider in sourceControlProviders)
            {
                Console.WriteLine(provider.Value);
            }
            Console.WriteLine("Source Control Provider: ");
            return Console.ReadLine();
        }

        private static string GetDomain()
        {
            var domains = domainsSection.GetChildren();
            Console.WriteLine("Available Domains: ");
            foreach (var domain in domains)
            {
                Console.WriteLine(domain.Key);
            }
            Console.WriteLine("Domain: ");
            return Console.ReadLine();
        }
    }
}
