﻿using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;

public class ReferenceFileRequest
{
    [Display("Project ID")]
    [DataSource(typeof(ProjectDataHandler))]
    public string ProjectUId { get; set; }
    
    [Display("Reference file ID")] 
    [DataSource(typeof(ReferenceFileDataHandler))]
    public string ReferenceFileUId { get; set; }
}