using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos
{
    public class UserDto
    {
        //public string Id { get; set; }
        [Display("First name")] public string FirstName { get; set; }
        [Display("Full name")] public string FullName => $"{FirstName} {LastName}";
        public string Role { get; set; }
        public bool Active { get; set; }
        [Display("User name")] public string UserName { get; set; }
        public bool Terminologist { get; set; }
        [Display("UID")] public string UId { get; set; }
        public string Note { get; set; }
        [Display("Last name")] public string LastName { get; set; }
        public string Timezone { get; set; }
        public string Email { get; set; }
    }
}