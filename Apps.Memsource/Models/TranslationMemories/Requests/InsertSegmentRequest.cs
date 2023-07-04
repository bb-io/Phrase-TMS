﻿namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class InsertSegmentRequest
    {
        public string TranslationMemoryUId { get; set; }

        public string TargetLanguage { get; set; }

        public string SourceSegment { get; set; }

        public string TargetSegment { get; set; }
    }
}
