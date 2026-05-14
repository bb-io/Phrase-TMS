using System.Net;
using RestSharp;
using System.Net.Http.Headers;
using Apps.PhraseTMS.Dtos.Auth;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Extensions;

public static class RestResponseExtensions
{
    public static string GetFilenameFromHeader(this RestResponse response, string fallbackName = "default_filename")
    {
        var filenameHeader = response?.ContentHeaders?
            .FirstOrDefault(h => h.Name != null && h.Name.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase));

        if (filenameHeader != null &&
            ContentDispositionHeaderValue.TryParse(filenameHeader.Value?.ToString(), out var contentDisposition))
        {
            var fileName = contentDisposition.FileNameStar ?? contentDisposition.FileName ?? fallbackName;
            fileName = fileName.Trim('\"');

            if (!string.IsNullOrEmpty(fileName))
                return Uri.UnescapeDataString(fileName);
        }

        return fallbackName;
    }
    
    public static bool IsInvalidGrant(this RestResponse response)
    {
        if (response.StatusCode != HttpStatusCode.BadRequest && response.StatusCode != HttpStatusCode.Unauthorized)
            return false;

        if (string.IsNullOrWhiteSpace(response.Content))
            return false;

        try
        {
            var error = JsonConvert.DeserializeObject<AuthErrorDto>(response.Content);
            return error?.Error == "invalid_grant";
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
