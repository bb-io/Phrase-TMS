using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Users.Requests;

public class ListAllUsersQuery
{
    [Display("First name")]
    public string? firstName { get; set; }

    [Display("Last name")]
    public string? lastName { get; set; }

    [Display("Name")]
    public string? name { get; set; }

    [Display("User name")]
    public string? userName { get; set; }

    [Display("Email")]
    public string? email { get; set; }

    [Display("Name or email")]
    public string? nameOrEmail { get; set; }

    [Display("Role")]
    public IEnumerable<string>? role { get; set; }

    [Display("Include deleted")]
    public bool? includeDeleted { get; set; }

    [Display("Sort")]
    public IEnumerable<string>? sort { get; set; }

    [Display("Order")]
    public IEnumerable<string>? order { get; set; }
}