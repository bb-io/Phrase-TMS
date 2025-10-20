using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Webhooks.Models.Requests
{
    public class JobCreatedFilters
    {
        [Display("Projects")]
        [DataSource(typeof(ProjectDataHandler))]
        public IEnumerable<string>? Projects { get; set; }

        [Display("Project owner", Description = "Owner id, username or email")]
        public string? ProjectOwner { get; set; }

        [Display("Domain")]
        public string? Domain { get; set; }

        [Display("SubDomain")]
        public string? SubDomain { get; set; }

        [Display("Client", Description = "Client id or name")]
        public string? Client { get; set; }
    }
}
