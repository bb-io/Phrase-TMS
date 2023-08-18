using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Apps.PhraseTMS.Constants;

public static class JsonConfig
{
    public static readonly JsonSerializerSettings Settings = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        },
        Formatting = Formatting.Indented
    };
}