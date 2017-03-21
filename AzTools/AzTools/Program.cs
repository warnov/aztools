using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzTools
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0) help();
            else
            switch(args[0].ToLower())
            {
                case "sc":
                    SC(args);
                    break;
                case "help":
                default:
                    help();
                    break;
            }
            Console.ReadKey();
        }

        private static void help()
        {
            Console.WriteLine("Available commands:\nsc: Assemble Azure Storage Connection String. Use: azTools sc [accountName] [pak]");            
        }

        private static void SC(string[] args)
        {
            var accountName = args[1];
            var PAK = args[2];
            Console.WriteLine($"Assembled connection string:\n\nDefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={PAK}");
        }
    }
}
