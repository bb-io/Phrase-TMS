﻿using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class ListAnalysesPathRequest
{
    [Display("Job UID")]
    [DataSource(typeof(JobDataHandler))]
    public string JobUId { get; set; }
}