using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos
{
    public class SegmentCountDto
    {
        [JsonProperty("segmentsCountsResults")]
        public List<SegmentsCountsResult> SegmentsCountsResults { get; set; }
    }

    public class Counts
    {
        [JsonProperty("allConfirmed")]
        public bool AllConfirmed { get; set; }

        [JsonProperty("charsCount")]
        public int CharsCount { get; set; }

        [JsonProperty("completedCharsCount")]
        public int CompletedCharsCount { get; set; }

        [JsonProperty("confirmedCharsCount")]
        public int ConfirmedCharsCount { get; set; }

        [JsonProperty("confirmedLockedCharsCount")]
        public int ConfirmedLockedCharsCount { get; set; }

        [JsonProperty("lockedCharsCount")]
        public int LockedCharsCount { get; set; }

        [JsonProperty("segmentsCount")]
        public int SegmentsCount { get; set; }

        [JsonProperty("completedSegmentsCount")]
        public int CompletedSegmentsCount { get; set; }

        [JsonProperty("lockedSegmentsCount")]
        public int LockedSegmentsCount { get; set; }

        [JsonProperty("segmentGroupsCount")]
        public int SegmentGroupsCount { get; set; }

        [JsonProperty("translatedSegmentsCount")]
        public int TranslatedSegmentsCount { get; set; }

        [JsonProperty("translatedLockedSegmentsCount")]
        public int TranslatedLockedSegmentsCount { get; set; }

        [JsonProperty("wordsCount")]
        public int WordsCount { get; set; }

        [JsonProperty("completedWordsCount")]
        public int CompletedWordsCount { get; set; }

        [JsonProperty("confirmedWordsCount")]
        public int ConfirmedWordsCount { get; set; }

        [JsonProperty("confirmedLockedWordsCount")]
        public int ConfirmedLockedWordsCount { get; set; }

        [JsonProperty("lockedWordsCount")]
        public int LockedWordsCount { get; set; }

        [JsonProperty("addedSegments")]
        public int AddedSegments { get; set; }

        [JsonProperty("addedWords")]
        public int AddedWords { get; set; }

        [JsonProperty("machineTranslationPostEditedSegmentsCount")]
        public int MachineTranslationPostEditedSegmentsCount { get; set; }

        [JsonProperty("machineTranslationRelevantSegmentsCount")]
        public int MachineTranslationRelevantSegmentsCount { get; set; }

        [JsonProperty("qualityAssurance")]
        public QualityAssurance QualityAssurance { get; set; }

        [JsonProperty("qualityAssuranceResolved")]
        public bool QualityAssuranceResolved { get; set; }
    }

    public class PreviousWorkflow
    {
        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonProperty("counts")]
        public Counts Counts { get; set; }
    }

    public class QualityAssurance
    {
        [JsonProperty("segmentsCount")]
        public int SegmentsCount { get; set; }

        [JsonProperty("warningsCount")]
        public int WarningsCount { get; set; }

        [JsonProperty("ignoredWarningsCount")]
        public int IgnoredWarningsCount { get; set; }
    }

    public class SegmentsCountsResult
    {
        [JsonProperty("jobPartUid")]
        public string JobPartUid { get; set; }

        [JsonProperty("counts")]
        public Counts Counts { get; set; }

        [JsonProperty("previousWorkflow")]
        public PreviousWorkflow PreviousWorkflow { get; set; }
    }
}
