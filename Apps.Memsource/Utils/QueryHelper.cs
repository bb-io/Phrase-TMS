using Newtonsoft.Json;

namespace Apps.PhraseTMS.Utils;

public static class QueryHelper
{
    public static string WithQuery(this string str, object? queryObj)
    {
        if (queryObj == null)
            return str;

        var queryParameters = new List<string>();
        var properties = queryObj.GetType().GetProperties();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(queryObj, null);
            if (value == null)
                continue;

            var jsonPropertyAttribute = prop.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                .Cast<JsonPropertyAttribute>()
                .FirstOrDefault();
            var key = jsonPropertyAttribute != null ? jsonPropertyAttribute.PropertyName : prop.Name;

            if (value is IEnumerable<string> enumerableValue && !(value is string))
            {
                foreach (var item in enumerableValue)
                {
                    if (item != null)
                    {
                        queryParameters.Add($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(item)}");
                    }
                }
            }
            else
            {
                queryParameters.Add($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value.ToString())}");
            }
        }

        var queryString = string.Join("&", queryParameters);
        return string.IsNullOrEmpty(queryString) ? str : $"{str}?{queryString}";
    }
}