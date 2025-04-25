using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class UpdateSourceResponse
    {
        [JsonProperty("jobs")]
        public IEnumerable<UpdateSourceJob> Jobs { get; set; }

    }

    public class UpdateSourceJob
    {
        [Display("Job ID")] public string Uid { get; set; }

        [Display("File name")] public string Filename { get; set; }

        public string Status { get; set; }

        [Display("Target language")] public string TargetLang { get; set; }
    }
}
