using System.IO;
using check_yo_self_api.Server;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using Microsoft.Extensions.Hosting;

//Generated on 03/04/2019 with Ecstatic Emu 1.0.6

namespace check_yo_self_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cross-platform way to set the machine name environment variable.
            // Serilog will only look for HOSTNAME which is not guaranteed to exist
            // on any platform.  With this call, we're making that guarantee.
            Environment.SetEnvironmentVariable("HOSTNAME", System.Environment.MachineName);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) => 
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => {
                    var appsettingsConfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables()
                        .Build();

                    config
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddEnvironmentVariables();

                    if (hostingContext.HostingEnvironment.IsDevelopment()) {
                        // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                        config.AddUserSecrets<check_yo_self_api.Startup>();
                    }
                })
                .ConfigureLogging((hostingContext, logging) => {
                    logging.ClearProviders();                    
                    check_yo_self_api.Server.Startup.Serilog.Setup(hostingContext);
                    logging.AddSerilog(dispose: true);
                })
                .UseConfiguration(
                  new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("hosting.json", optional: true)
                    .AddCommandLine(args)
                    .AddEnvironmentVariables()
                    .Build()
                )
                .UseStartup<check_yo_self_api.Startup>()
                .Build();
    }
}
