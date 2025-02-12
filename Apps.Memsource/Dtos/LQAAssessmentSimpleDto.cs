
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos
{
    public class LQAAssessmentSimpleDto
    {
        [JsonProperty("assessmentDetails")]
        public List<AssessmentDetail> AssessmentDetails { get; set; }
    }

    public class AssessmentDetail
    {
        [JsonProperty("lqaEnabled")]
        public bool LqaEnabled { get; set; }

        [JsonProperty("assessmentResult")]
        public object AssessmentResult { get; set; }

        [JsonProperty("jobPartUid")]
        public string JobPartUid { get; set; }
    }

}
