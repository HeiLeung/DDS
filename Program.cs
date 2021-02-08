using Microsoft.Extensions.Configuration;
using Serilog;
using System.Diagnostics;
using System.IO;

namespace Arrow
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///[STAThread]
        ///static void Main()
        static void Main(string[] args)
        {
            // create config
            var config = new ConfigurationBuilder()
                .SetBasePath(GetBasePath())
                .AddJsonFile("config.json")
                .Build();

            // Create logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            /*
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(config["Log"])
                .CreateLogger();
            log.Information("Hello, testing serilog");           
            */
            bool connectToRoot = bool.Parse(config["DDS:ConnectToRoot"]);

            DdsIpPortConnObj pub = new DdsIpPortConnObj(config["DDS:Pub:IP"], int.Parse(config["DDS:Pub:Port"]), int.Parse(config["DDS:Pub:HWM"]));
            DdsIpPortConnObj sub = new DdsIpPortConnObj(config["DDS:Sub:IP"], int.Parse(config["DDS:Sub:Port"]), int.Parse(config["DDS:Sub:HWM"]));
            DdsIpPortConnObj router = new DdsIpPortConnObj(config["DDS:Router:IP"], int.Parse(config["DDS:Router:Port"]), int.Parse(config["DDS:Router:HWM"]));
            DdsIpPortConnObj root = new DdsIpPortConnObj(config["DDS:Root:IP"], int.Parse(config["DDS:Root:Port"]), int.Parse(config["DDS:Root:HWM"]));

            if (connectToRoot)
            {
                // start DDS with connection to root DDS
                DDS dds = new DDS();
                dds.Start(sub, router, pub, root);
            }
            else
            {
                DDS dds = new DDS();
                dds.Start(sub, router, pub);
            }
        }

        static string GetBasePath()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
    }
}
