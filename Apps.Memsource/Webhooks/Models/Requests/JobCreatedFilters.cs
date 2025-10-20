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
        [DataSource(typeof(UserDataHandler))]
        public string? ProjectOwner { get; set; }

        [Display("Domain")]
        [DataSource(typeof(DomainDataHandler))]
        public string? Domain { get; set; }

        [Display("SubDomain")]
        [DataSource(typeof(SubdomainDataHandler))]
        public string? SubDomain { get; set; }

        [Display("Client", Description = "Client id or name")]
        [DataSource(typeof(ClientDataHandler))]
        public string? Client { get; set; }
    }
}
