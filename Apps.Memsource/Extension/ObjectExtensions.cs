using Apps.PhraseTMS.Utils.Converters;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Extension;

public static class ObjectExtensions
{
    public static Dictionary<string, string> AsDictionary(this object obj)
    {
        var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            Converters = { new StringValueConverter() },
        });

        return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ??
               throw new($"Could not process input {json}");
    }
}