using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class PreTranslateJobResponse
    {
        [Display("Request info")]
        [JsonProperty("asyncRequest")]
        public AsyncRequestDto? AsyncRequest { get; set; }      
    }

    public class AsyncRequestDto
    {
        [Display("Request ID")]
        [JsonProperty("id")]
        public string? Id { get; set; }

        [Display("Creator")]
        [JsonProperty("createdBy")]
        public UserPretranslationDto? CreatedBy { get; set; }

        [Display("Creation date")]
        [JsonProperty("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [Display("Action")]
        [JsonProperty("action")]
        public string? Action { get; set; }

        [Display("Project info")]
        [JsonProperty("project")]
        public ProjectReference? Project { get; set; }
    }
    public class ProjectReference
    {
        [Display("Project name")]
        [JsonProperty("name")]
        public string? Name { get; set; }

        [Display("Project ID")]
        [JsonProperty("uid")]
        public string? Uid { get; set; }
    }
}
