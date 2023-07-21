using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Users.Requests;

public class ListAllUsersQuery
{
    [Display("First name")]
    public string? FirstName { get; set; }

    [Display("Last name")]
    public string? LastName { get; set; }

    [Display("Name")]
    public string? Name { get; set; }

    [Display("User name")]
    public string? UserName { get; set; }

    [Display("Email")]
    public string? Email { get; set; }

    [Display("Name or email")]
    public string? NameOrEmail { get; set; }

    [Display("Role")]
    public IEnumerable<string>? Role { get; set; }

    [Display("Include deleted")]
    public bool IncludeDeleted { get; set; }

    [Display("Sort")]
    public IEnumerable<string>? Sort { get; set; }

    [Display("Order")]
    public IEnumerable<string>? Order { get; set; }
}