using Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class ListAllProjectsQuery
{
    [Display("Name")] public string? Name { get; set; }
    [Display("Client ID")] public long? ClientId { get; set; }
    [Display("Client name")] public string? ClientName { get; set; }
    [Display("Business unit ID")] public long? BusinessUnitId { get; set; }
    [Display("Business unit name")] public string? BusinessUnitName { get; set; }
    [Display("Statuses")] public IEnumerable<string>? Statuses { get; set; }
    [Display("Target languages")] public IEnumerable<string>? TargetLangs { get; set; }
    [Display("Domain ID")] public long? DomainId { get; set; }
    [Display("Domain name")] public string? DomainName { get; set; }
    [Display("Subdomain ID")] public long? SubDomainId { get; set; }
    [Display("Subdomain name")] public string? SubDomainName { get; set; }
    [Display("Cost Center ID")] public long? CostCenterId { get; set; }
    [Display("Cost center name")] public string? CostCenterName { get; set; }
    [Display("Due in hours")] public int? DueInHours { get; set; }
    [Display("Created in last hours")] public int? CreatedInLastHours { get; set; }
    [Display("Source languages")] public IEnumerable<string>? SourceLangs { get; set; }
    [Display("Owner ID")] public long? OwnerId { get; set; }
    [Display("Job statuses")] public IEnumerable<string>? JobStatuses { get; set; }
    
    [Display("Job status group")] 
    [DataSource(typeof(JobStatusGroupDataHandler))]
    public string? JobStatusGroup { get; set; }
    
    [Display("Buyer ID")] public long? BuyerId { get; set; }
    [Display("Name or internal ID")] public string? NameOrInternalId { get; set; }
    [Display("Include archived")] public bool? IncludeArchived { get; set; }
    [Display("Archived only")] public bool? ArchivedOnly { get; set; }
}