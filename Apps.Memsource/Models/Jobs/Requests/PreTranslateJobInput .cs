﻿using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class PreTranslateJobInput
    {
        [Display("Job IDs")]
        [DataSource(typeof(JobDataHandler))]
        public IEnumerable<string> Jobs { get; set; }

        [Display("Segment filters")]
        [StaticDataSource(typeof(SegmentDataHandler))]
        public IEnumerable<string>? SegmentFilters { get; set; }

        [Display("Use project pre-translate settings?")]
        public bool? UseProjectPreTranslateSettings { get; set; }

        [Display("Callback URL")]
        public string? CallbackUrl { get; set; }
    }
}
