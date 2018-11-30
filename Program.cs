using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using log4net;
using log4net.Config;
using System.Reflection;

namespace ConfigureSetting
{
    public class Program
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(Program));
        public static void Main(string[] args)
        {
            LoadLog4netConfig();
            _log.Info("Application Start");
            NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            CreateWebHostBuilder(args).Build().Run();
        }

        //remember to add parameters SiteName and Domain in commandline
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    var env = hostContext.HostingEnvironment;
                    config.SetBasePath(env.ContentRootPath)
                        .AddJsonFile(path:"settings.json", optional:false, reloadOnChange:true)
                        .AddJsonFile(path:$"settings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    //Path.Combine(env.ContentRootPath, "Configuration"))
                    config.AddEnvironmentVariables();
                    var dictionary = new Dictionary<string, string>
                    {
                        {"Site:Name", "Configure WEB"},
                        {"Site:Domain", "Dictionary"}
                    };
                    config.AddInMemoryCollection(dictionary);
                    config.Add(new CustomConfigurationSource());
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    var env = hostContext.HostingEnvironment;
                    var configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(env.ContentRootPath))
                    .AddJsonFile(reloadOnChange:true, optional:true, path:"settings.json")
                    .Build();
                    logging.AddConfiguration(configuration.GetSection("Logging"));
                    logging.AddProvider(new Log4netProvider("log4net.config"));
                })
                .UseStartup<Startup>()
                .UseNLog();

        private static void LoadLog4netConfig()
        {
            var repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy)
            );
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }
    }
}
