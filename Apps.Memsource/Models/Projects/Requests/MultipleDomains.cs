using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class MultipleDomains
{
    [Display("Domain IDs")]
    [DataSource(typeof(DomainDataHandler))]
    public IEnumerable<string>? Domains { get; set; } = null;
}

