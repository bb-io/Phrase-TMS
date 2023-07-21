using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace Apps.PhraseTMS.Extension;

public static class RestRequestExtensions
{
    public static RestRequest WithJsonBody(this RestRequest request, object body)
    {
        var json = JsonConvert.SerializeObject(body, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        });

        return request.AddJsonBody(json);
    }
}