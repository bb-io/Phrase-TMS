using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class MultipleSubdomains
{
    [Display("Subdomain IDs")]
    [DataSource(typeof(SubdomainDataHandler))]
    public IEnumerable<string>? Subdomains { get; set;}
}

