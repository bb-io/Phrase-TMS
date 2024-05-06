using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Users.Requests;

public class GetUserRequest
{
    [Display("User UID")]
    [DataSource(typeof(UserDataHandler))]
    public string UserUId { get; set; }
}