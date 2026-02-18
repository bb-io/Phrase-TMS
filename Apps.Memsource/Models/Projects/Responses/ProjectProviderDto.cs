using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Projects.Responses
{
    public class ProjectProviderDto
    {
        [Display("Type")]
        [JsonProperty("type")]
        public string? Type { get; set; }

        [Display("ID")]
        [JsonProperty("id")]
        public string? Id { get; set; }

        [Display("UID")]
        [JsonProperty("uid")]
        public string? Uid { get; set; }
    }

    public class ProjectProvidersPageResponse
    {
        [JsonProperty("totalElements")]
        public int TotalElements { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("numberOfElements")]
        public int NumberOfElements { get; set; }

        [JsonProperty("content")]
        public List<ProjectProviderDto> Content { get; set; } = new();
    }

    public class GetProjectProvidersResponse
    {
        public List<ProjectProviderDto> Providers { get; set; } = new();

        [Display("Total quantity")]
        public int? TotalElements { get; set; }
    }
}
