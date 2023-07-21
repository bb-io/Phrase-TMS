using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Users.Requests
{
    public class GetUserRequest
    {
        [Display("User UID")]
        public string UserUId { get; set; }
    }
}
