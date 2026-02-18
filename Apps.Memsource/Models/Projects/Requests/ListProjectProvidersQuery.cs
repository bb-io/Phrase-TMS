using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class ListProjectProvidersQuery
    {
        [Display("Provider name")]
        public string? ProviderName { get; set; }
    }
}
