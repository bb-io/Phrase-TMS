﻿using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class JobOptionalRequest
{
    [Display("Job ID")]
    [DataSource(typeof(JobDataHandler))] 
    public string? JobUId { get; set; }
}