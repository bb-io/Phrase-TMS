using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Responses
{
    public class GetProjectResponse
    {
        public string Name { get; set; }

        [Display("ID")]
        public string Id { get; set; }

        [Display("Creation date")]
        public string DateCreated { get; set; }
    }
}
