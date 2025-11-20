using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class SearchProjectTemplatesQuery
    {
        [Display("Template name")]
        public string? Name { get; set; }

        [Display("Client ID")]
        public int? ClientId { get; set; }

        [Display("Client name")]
        public string? ClientName { get; set; }

        [Display("Owner UID")]
        public string? OwnerUid { get; set; }

        [Display("Created by UID")]
        public string? CreatedByUid { get; set; }

        [Display("Domain name")]
        public string? DomainName { get; set; }

        [Display("Subdomain name")]
        public string? SubDomainName { get; set; }

        [Display("Cost center ID")]
        public int? CostCenterId { get; set; }

        [Display("Cost center name")]
        public string? CostCenterName { get; set; }

        [Display("Business unit name")]
        public string? BusinessUnitName { get; set; }

        [Display("Source languages")]
        public IEnumerable<string>? SourceLangs { get; set; }

        [Display("Target languages")]
        public IEnumerable<string>? TargetLangs { get; set; }

        [Display("Created in last X hours")]
        public int? CreatedInLastHours { get; set; }

        [Display("Sort by")]
        public string? Sort { get; set; }

        [Display("Sort direction")]
        public string? Direction { get; set; }
    }
}
