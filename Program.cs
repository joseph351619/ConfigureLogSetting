using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConfigureSetting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    var env = hostContext.HostingEnvironment;
                    config.SetBasePath(env.ContentRootPath)
                        .AddJsonFile(path:"setting.json", optional:false, reloadOnChange:true);
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
                .UseStartup<Startup>();
    }
}
