using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Filters;

namespace MyWebsite{
public class UserController : Controller
{
    private readonly IConfiguration _config;
        private readonly Settings _settings;
    private readonly ILogger _logger;
    public UserController(IConfiguration config, IOptions<Settings> settings, ILogger<UserController> logger)
    {
        _config = config;
        _settings = settings.Value;
        _logger = logger;
    }

    ///<summary>
    ///testing
    ///</summary>
        [TypeFilter(typeof(ActionFilter))]
    public void index()
    {
            _logger.LogTrace("This trace log from Home.Index()");
            _logger.LogDebug("This debug log from Home.Index()");
            _logger.LogInformation("This information log from Home.Index()");
            _logger.LogWarning("This warning log from Home.Index()");
            _logger.LogError("This error log from Home.Index()");
            _logger.LogCritical("This critical log from Home.Index()");


            var defaultCulture = _config["SupportedCultures:1"];
            var subProperty1 = _config["CustomObject:Property:SubProperty1"];
            var subProperty2 = _config["CustomObject:Property:SubProperty2"];
            var subProperty3 = _config["CustomObject:Property:SubProperty3"];
            var _defaultCulture = _settings.SupportedCultures[1];
            var _subProperty1 = _settings.CustomObject.Property.SubProperty1;
            var _subProperty2 = _settings.CustomObject.Property.SubProperty2;
            var _subProperty3 = _settings.CustomObject.Property.SubProperty3;
            var siteName = _config["SiteName"];
            var domain = _config["Domain"];
            var sample = _config["Sample"];
            var _siteName = _config["Site:Name"];
            var _domain = _config["Site:Domain"];
            var __siteName = _config["Custom:Site:Name"];
            var __domain = _config["Custom:Site:Domain"];

            var _environmentDevelop = _config["NewObject:Property:SubProperty3"];

            Response.WriteAsync(
                $"defaultCulture({defaultCulture.GetType()}): {defaultCulture}\r\n"
                + $"subProperty1({subProperty1.GetType()}): {subProperty1}\r\n"
                + $"subProperty2({subProperty2.GetType()}): {subProperty2}\r\n"
                + $"subProperty3({subProperty3.GetType()}): {subProperty3}\r\n"
                + $"_defaultCulture({_defaultCulture.GetType()}): {_defaultCulture}\r\n"
                + $"_subProperty1({_subProperty1.GetType()}): {_subProperty1}\r\n"
                + $"_subProperty2({_subProperty2.GetType()}): {_subProperty2}\r\n"
                + $"_subProperty3({_subProperty3.GetType()}): {_subProperty3}\r\n"
                // + $"SiteName({siteName.GetType()}): {siteName}\r\n"
                // + $"Domain({domain.GetType()}): {domain}\r\n"
                +$"sample({sample.GetType()}): {sample}\r\n"
                + $"_Site.Name({_siteName.GetType()}): {_siteName}\r\n"
                + $"_Site.Domain({_domain.GetType()}): {_domain}\r\n"
                + $"Custom.Site.Name({__siteName.GetType()}): {__siteName}\r\n"
                + $"Custom.Site.Domain({__domain.GetType()}): {__domain}\r\n"
                + $"environmentDevelop:{_environmentDevelop}\r\n"
            );
    }

    [Route("/test")]
        [TypeFilter(typeof(ActionFilter))]
        public string Test()
        {
            throw new System.Exception("This is exception sample from Test().");
        }

[Route("/error")]
        [TypeFilter(typeof(ActionFilter))]
    public IActionResult Error()
    {
            return View();
        // throw new System.Exception("Error");
    }

}
}