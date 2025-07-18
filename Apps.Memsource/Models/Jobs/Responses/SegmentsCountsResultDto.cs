using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class GetSegmentsCountResponse
    {
        [Display("Segments counts results")]
        public List<SegmentsCountsResultDto> SegmentsCountsResults { get; set; }
    }

    public class SegmentsCountsResultDto
    {
        [JsonProperty("jobPartUid")]
        [Display("Job part UID")]
        public string JobPartUid { get; set; }

        [JsonProperty("counts")]
        [Display("Current counts")]
        public CountsDto Counts { get; set; }

        [JsonProperty("previousWorkflow")]
        [Display("Previous workflow step data")]
        public PreviousWorkflowDto PreviousWorkflow { get; set; }
    }

    public class CountsDto
    {
        [JsonProperty("allConfirmed")][Display("All confirmed")] public bool AllConfirmed { get; set; }
        [JsonProperty("charsCount")][Display("Chars count")] public int CharsCount { get; set; }
        [JsonProperty("completedCharsCount")][Display("Completed chars count")] public int CompletedCharsCount { get; set; }
        [JsonProperty("confirmedCharsCount")][Display("Confirmed char count")] public int ConfirmedCharsCount { get; set; }
        [JsonProperty("lockedCharsCount")][Display("Locked chars count")] public int LockedCharsCount { get; set; }
        [JsonProperty("segmentsCount")][Display("Segments count")] public int SegmentsCount { get; set; }
        [JsonProperty("completedSegmentsCount")][Display("Completed  segments count")] public int CompletedSegmentsCount { get; set; }
        [JsonProperty("lockedSegmentsCount")][Display("Locked segments count")] public int LockedSegmentsCount { get; set; }
        [JsonProperty("translatedSegmentsCount")][Display("Translated segments count")] public int TranslatedSegmentsCount { get; set; }
        [JsonProperty("wordsCount")][Display("Words count")] public int WordsCount { get; set; }
        [JsonProperty("qualityAssurance")][Display("Quality assurance")] public QualityAssuranceDto QualityAssurance { get; set; }
        [JsonProperty("qualityAssuranceResolved")][Display("Quality assurance resolved")] public bool QualityAssuranceResolved { get; set; }
    }

    public class PreviousWorkflowDto
    {
        [JsonProperty("completed")][Display("Completed")] public bool Completed { get; set; }
        [JsonProperty("counts")][Display("Counts")] public CountsDto Counts { get; set; }
    }

    public class QualityAssuranceDto
    {
        [JsonProperty("segmentsCount")][Display("Segment count")] public int SegmentsCount { get; set; }
        [JsonProperty("warningsCount")][Display("Warnings count")] public int WarningsCount { get; set; }
        [JsonProperty("ignoredWarningsCount")][Display("Ignored warning count")] public int IgnoredWarningsCount { get; set; }
    }
}
