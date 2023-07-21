using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests;

public class ListLQAProfilesQuery
{
    [Display("Name")] public string? Name { get; set; }
    [Display("Created by")] public string? CreatedBy { get; set; }
    [Display("Date created")] public string? DateCreated { get; set; }
    [Display("Sort")] public IEnumerable<string>? Sort { get; set; }
    [Display("Order")] public IEnumerable<string>? Order { get; set; }
}