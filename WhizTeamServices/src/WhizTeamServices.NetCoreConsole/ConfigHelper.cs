using Microsoft.Extensions.Configuration;

namespace WhizTeamServices.NetCoreConsole
{
    public static class ConfigHelper
    {
        public static IConfiguration Config
        {
            get
            {
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                return builder.Build();
            }
        }
    }
}
