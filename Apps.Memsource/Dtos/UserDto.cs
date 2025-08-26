using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class UserDto
{
    [Display("User ID")]
    [JsonProperty("uid")]
    public string UId { get; set; }

    [Display("First name")] 
    public string FirstName { get; set; }
    [Display("Last name")] 
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool Active { get; set; }

    [Display("User name")] 
    public string UserName { get; set; }
    public bool Terminologist { get; set; }
    public string Note { get; set; }    
    public string Timezone { get; set; }

    [JsonProperty("dateDeleted")]
    [Display("Date deleted")]
    public DateTimeOffset? DateDeleted { get; set; }

    [JsonProperty("dateCreated")]
    [Display("Date created")]
    public DateTimeOffset? DateCreated { get; set; }

}