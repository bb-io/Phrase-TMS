using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class SearchTermBasesQuery
    {
        [Display("Name")]
        public string? Name { get; set; }

        [Display("Language code")]
        [DataSource(typeof(LanguageDataHandler))]
        public IEnumerable<string>? Languages { get; set; }

        [Display("Client ID")]
        [DataSource(typeof(ClientDataHandler))]
        public string? ClientId { get; set; }

        [Display("Domain ID")]
        [DataSource(typeof(DomainDataHandler))]
        public string? DomainId { get; set; }

        [Display("Subdomain ID")]
        [DataSource(typeof(SubdomainDataHandler))]
        public string? SubDomainId { get; set; }

        [Display("Business unit ID")]
        [DataSource(typeof(BusinessUnitDataHandler))]
        public string? BusinessUnitId { get; set; }
    }
}
