using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Files.Requests;

public class ListFilesQuery
{
    public string? Name { get; set; }
    [Display("Created by")] public int? CreatedBy { get; set; }
    [Display("Bigger than")] public int? BiggerThan { get; set; }
}