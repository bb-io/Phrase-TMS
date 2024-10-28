using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class ListAllProjectsQuery
{
    [Display("Name"), JsonProperty("name")] public string? Name { get; set; }
    [Display("Client ID"), JsonProperty("clientId")] public long? ClientId { get; set; }
    [Display("Client name"), JsonProperty("clientName")] public string? ClientName { get; set; }
    [Display("Business unit ID"), JsonProperty("businessUnitId")] public long? BusinessUnitId { get; set; }
    [Display("Business unit name"), JsonProperty("businessUnitName")] public string? BusinessUnitName { get; set; }
    [Display("Statuses"), JsonProperty("statuses")] public IEnumerable<string>? Statuses { get; set; }
    [Display("Target languages"), JsonProperty("targetLangs")] public IEnumerable<string>? TargetLangs { get; set; }
    [Display("Domain ID"), JsonProperty("domainId")] public long? DomainId { get; set; }
    [Display("Domain name"), JsonProperty("domainName")] public string? DomainName { get; set; }
    [Display("Subdomain ID"), JsonProperty("subDomainId")] public long? SubDomainId { get; set; }
    [Display("Subdomain name"), JsonProperty("subDomainName")] public string? SubDomainName { get; set; }
    [Display("Cost Center ID"), JsonProperty("costCenterId")] public long? CostCenterId { get; set; }
    [Display("Cost center name"), JsonProperty("costCenterName")] public string? CostCenterName { get; set; }
    [Display("Due in hours"), JsonProperty("dueInHours")] public int? DueInHours { get; set; }
    [Display("Created in last hours"), JsonProperty("createdInLastHours")] public int? CreatedInLastHours { get; set; }
    [Display("Source languages"), JsonProperty("sourceLangs")] public IEnumerable<string>? SourceLangs { get; set; }
    [Display("Owner ID"), JsonProperty("ownedId")] public long? OwnerId { get; set; }
    [Display("Job statuses"), JsonProperty("jobStatuses")] public IEnumerable<string>? JobStatuses { get; set; }
    
    [Display("Job status group"), StaticDataSource(typeof(JobStatusGroupDataHandler)), JsonProperty("jobStatusGroup")]
    public string? JobStatusGroup { get; set; }
    
    [Display("Buyer ID"), JsonProperty("buyerId")] public long? BuyerId { get; set; }
    [Display("Name or internal ID"), JsonProperty("nameOrInternalId")] public string? NameOrInternalId { get; set; }
    [Display("Include archived"), JsonProperty("includeArchived")] public bool? IncludeArchived { get; set; }
    [Display("Archived only"), JsonProperty("archivedOnly")] public bool? ArchivedOnly { get; set; }
}