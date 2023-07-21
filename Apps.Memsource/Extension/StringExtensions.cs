using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Apps.PhraseTMS.Extension;

public static class StringExtensions
{
    public static string ToCamelCase(this string str)
        => JsonNamingPolicy.CamelCase.ConvertName(str);

    public static string WithQuery(this string str, object queryObj)
    {
        var query = queryObj.AsDictionary().WithoutNulls();

        return QueryHelpers.AddQueryString(str, query);
    }
}