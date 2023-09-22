﻿using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class CreateJobRequest : ProjectRequest
{
    [Display("Target languages")] 
    public IEnumerable<string> TargetLanguages { get; set; }

    [Display("File")]
    public File File { get; set; }
        
}