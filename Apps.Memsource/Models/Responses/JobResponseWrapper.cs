using Apps.PhraseTMS.Dtos.Async;
using Apps.PhraseTMS.Dtos.Jobs;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Responses;

public class JobResponseWrapper
{
    [Display("File name")]
    public string? Filename => Jobs.FirstOrDefault()?.Filename;

    [Display("Source file ID")]
    public string? SourceFileUid => Jobs.FirstOrDefault()?.SourceFileUid;

    [JsonProperty("asyncRequest")]
    public AsyncRequest? AsyncRequest { get; set; }

    [Display("Unsupported files")]
    [JsonProperty("unsupportedFiles")]
    public IEnumerable<string> UnsupportedFiles { get; set; } = [];

    [Display("Warnings")]
    [JsonProperty("warnings")]
    public IEnumerable<JobCreationWarning> Warnings { get; set; } = [];

    public IEnumerable<CreatedJobDto> Jobs { get; set; } = [];
}

public class JobCreationWarning
{
    [Display("Domain")]
    public string? Domain { get; set; }

    [Display("Message")]
    public string? Message { get; set; }
}
