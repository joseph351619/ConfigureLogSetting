using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
public class CustomConfigurationSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new CustomConfigurationProvider();
    }
}

public class CustomConfigurationProvider : ConfigurationProvider 
{
    public override void Load()
    {
        Data = new Dictionary<string, string>
            {
                { "Custom:Site:Name", "Dictionary" },
                { "Custom:Site:Domain", "_Domain" }
            };
    }
}