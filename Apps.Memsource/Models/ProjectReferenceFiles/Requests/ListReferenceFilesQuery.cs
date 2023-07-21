using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;

public class ListReferenceFilesQuery
{
    [Display("File name")]
    public string? Filename { get; set; }

    [Display("Date created since")]
    public string? DateCreatedSince { get; set; }

    [Display("Created by")]
    public string? CreatedBy { get; set; }

    [Display("Sort")]
    public string? Sort { get; set; }

    [Display("Order")]
    public string? Order { get; set; }
}