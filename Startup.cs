using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Filters;
using System.Text.RegularExpressions;


namespace ConfigureSetting
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                config =>
            {
                config.Filters.Add(new ResultFilter());
                config.Filters.Add(new ExceptionFilter());
                config.Filters.Add(new ResourceFilter());
            }
            );
            services.Configure<Settings>(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        app.UseExceptionHandler(new ExceptionHandlerOptions()
        {
            ExceptionHandler = async context =>
            {
                bool isApi = Regex.IsMatch(context.Request.Path.Value, "^/api/", RegexOptions.IgnoreCase);
                if (isApi)
                {
                    context.Response.ContentType = "application/json";
                    var json = @"{ ""Message"": ""Internal Server Error"" }";
                    await context.Response.WriteAsync(json);
                    return;
                }
                context.Response.Redirect("/error");
            }
        });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else if(env.IsEnvironment("test")){

            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMvcWithDefaultRoute();

            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
        }
    }
}
