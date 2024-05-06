using Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Users.Requests;

public class AddUserRequest
{
    public string Email { get; set; }
    
    [Display("First name")]
    public string FirstName { get; set; }
    
    [Display("Last name")]
    public string LastName { get; set; }
    
    public string Password { get; set; }
    
    [StaticDataSource(typeof(RoleDataHandler))]
    public string Role { get; set; }
    
    [StaticDataSource(typeof(TimezoneDataHandler))]
    public string Timezone { get; set; }
    
    [Display("Username")]
    public string UserName { get; set; }
    
    [Display("Receive newsletter")]
    public bool? ReceiveNewsletter { get; set; }
    
    public string? Note { get; set; }
    
    [Display("Is active")]
    public bool? Active { get; set; }
}