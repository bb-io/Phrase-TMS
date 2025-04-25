using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Responses;

public class JobResponseWrapper
{
    [Display("File name")]
    public string? Filename => Jobs.FirstOrDefault()?.Filename;

    [Display("Source file ID")]
    public string? SourceFileUid => Jobs.FirstOrDefault()?.SourceFileUid;

    public IEnumerable<CreatedJobDto> Jobs { get; set; }
}