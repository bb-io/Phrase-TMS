using System.Reflection;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Utils.Converters;

public class StringValueConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException("This method is not implemented as the converter is for writing only.");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        writer.WriteStartObject();

        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);
            writer.WritePropertyName(GetJsonPropertyName(property).ToCamelCase());
            writer.WriteValue(propertyValue);
        }

        writer.WriteEndObject();
    }

    private string GetJsonPropertyName(PropertyInfo property)
    {
        var jsonPropertyName = property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
        return jsonPropertyName ?? property.Name;
    }
}