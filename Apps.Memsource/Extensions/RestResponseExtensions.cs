using RestSharp;
using System.Net.Http.Headers;

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
}
