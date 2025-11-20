namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class SearchProjectTemplatesApiQuery
    {
        public string? name { get; set; }
        public int? clientId { get; set; }
        public string? clientName { get; set; }
        public string? ownerUid { get; set; }
        public string? createdByUid { get; set; }
        public string? domainName { get; set; }
        public string? subDomainName { get; set; }
        public int? costCenterId { get; set; }
        public string? costCenterName { get; set; }
        public string? businessUnitName { get; set; }
        public IEnumerable<string>? sourceLangs { get; set; }
        public IEnumerable<string>? targetLangs { get; set; }
        public int? createdInLastHours { get; set; }
        public string? sort { get; set; }
        public string? direction { get; set; }

        public SearchProjectTemplatesApiQuery(SearchProjectTemplatesQuery query)
        {
            if (query == null)
                return;

            name = query.Name;
            clientId = query.ClientId;
            clientName = query.ClientName;
            ownerUid = query.OwnerUid;
            createdByUid = query.CreatedByUid;
            domainName = query.DomainName;
            subDomainName = query.SubDomainName;
            costCenterId = query.CostCenterId;
            costCenterName = query.CostCenterName;
            businessUnitName = query.BusinessUnitName;
            sourceLangs = query.SourceLangs;
            targetLangs = query.TargetLangs;
            createdInLastHours = query.CreatedInLastHours;
            sort = query.Sort;
            direction = query.Direction;
        }
    }
}
