using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class SearchTranslationMemoryRequest
    {
        [Display("Name (contains)")]
        public string? Name { get; set; }

        [Display("Source language")]
        [DataSource(typeof(LanguageDataHandler))]
        public string? SourceLang { get; set; }

        [Display("Target language")]
        [DataSource(typeof(LanguageDataHandler))]
        public string? TargetLang { get; set; }

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
