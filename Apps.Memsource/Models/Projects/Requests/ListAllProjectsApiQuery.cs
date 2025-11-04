using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class ListAllProjectsApiQuery
    {
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("clientId")] public long? ClientId { get; set; }
        [JsonProperty("clientName")] public string? ClientName { get; set; }
        [JsonProperty("businessUnitId")] public long? BusinessUnitId { get; set; }
        [JsonProperty("businessUnitName")] public string? BusinessUnitName { get; set; }
        [JsonProperty("statuses")] public IEnumerable<string>? Statuses { get; set; }
        [JsonProperty("targetLangs")] public IEnumerable<string>? TargetLangs { get; set; }
        [JsonProperty("domainId")] public long? DomainId { get; set; }
        [JsonProperty("domainName")] public string? DomainName { get; set; }
        [JsonProperty("subDomainId")] public long? SubDomainId { get; set; }
        [JsonProperty("subDomainName")] public string? SubDomainName { get; set; }
        [JsonProperty("costCenterId")] public long? CostCenterId { get; set; }
        [JsonProperty("costCenterName")] public string? CostCenterName { get; set; }
        [JsonProperty("dueInHours")] public int? DueInHours { get; set; }
        [JsonProperty("createdInLastHours")] public int? CreatedInLastHours { get; set; }
        [JsonProperty("sourceLangs")] public IEnumerable<string>? SourceLangs { get; set; }
        [JsonProperty("ownedId")] public long? OwnerId { get; set; }
        [JsonProperty("jobStatuses")] public IEnumerable<string>? JobStatuses { get; set; }
        [JsonProperty("jobStatusGroup")] public string? JobStatusGroup { get; set; }
        [JsonProperty("buyerId")] public long? BuyerId { get; set; }
        [JsonProperty("nameOrInternalId")] public string? NameOrInternalId { get; set; }
        [JsonProperty("includeArchived")] public bool? IncludeArchived { get; set; }
        [JsonProperty("archivedOnly")] public bool? ArchivedOnly { get; set; }
        [JsonProperty("sort")] public string? Sort { get; set; }
        [JsonProperty("order")] public string? Order { get; set; }

        public ListAllProjectsApiQuery(ListAllProjectsQuery q)
        {
            Name = q.Name;
            ClientId = q.ClientId;
            ClientName = q.ClientName;
            BusinessUnitId = q.BusinessUnitId;
            BusinessUnitName = q.BusinessUnitName;
            Statuses = q.Statuses;
            TargetLangs = q.TargetLangs;
            DomainId = q.DomainId;
            DomainName = q.DomainName;
            SubDomainId = q.SubDomainId;
            SubDomainName = q.SubDomainName;
            CostCenterId = q.CostCenterId;
            CostCenterName = q.CostCenterName;
            DueInHours = q.DueInHours;
            CreatedInLastHours = ToIntChecked(q.CreatedInLastHours);
            SourceLangs = q.SourceLangs;
            OwnerId = q.OwnerId;
            JobStatuses = q.JobStatuses;
            JobStatusGroup = q.JobStatusGroup;
            BuyerId = q.BuyerId;
            NameOrInternalId = q.NameOrInternalId;
            IncludeArchived = q.IncludeArchived;
            ArchivedOnly = q.ArchivedOnly;
            Sort = q.Sort;
            Order = q.Order;
        }

        private static int? ToIntChecked(double? v)
        {
            if (v is null) return null;

            var d = v.Value;
            if (double.IsNaN(d) || double.IsInfinity(d))
                throw new PluginMisconfigurationException("\"Created in last hours\" must be a finite number.");

            if (d < 0)
                throw new PluginMisconfigurationException("\"Created in last hours\" must be 0 or a positive integer.");

            if (Math.Abs(d - Math.Truncate(d)) > double.Epsilon)
                throw new PluginMisconfigurationException("\"Created in last hours\" must be an integer (e.g., 1, 2, 24).");

            return (int)d;
        }
    }
}
