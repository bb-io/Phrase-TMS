﻿using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class JobDto
{
    [Display("ID")] public string Uid { get; set; }

    [Display("File name")] public string Filename { get; set; }

    public string Status { get; set; }

    [Display("Inner ID", Description = "InnerId is a sequential number of a job in a project.\nJobs created from the same file share the same innerId across workflow steps.")]
    public string InnerId { get; set; }

    [Display("Target language")] 
    public string TargetLang { get; set; }

    [Display("Source language")]
    public string SourceLang { get; set; }

    [Display("Word count")]
    public int WordsCount { get; set; }

    public ProjectDto Project { get; set; }

    [Display("Assigned to")]
    public IEnumerable<Provider>? providers { get; set; }

    [Display("Workflow step"), JsonProperty("workflowStep")]
    public WorkflowStep WorkflowStep { get; set; }
}

public class WorkflowStep
{
    [Display("Workflow step name")]
    public string Name { get; set; } = string.Empty;
    
    [Display("Workflow step ID")]
    public string Id { get; set; } = string.Empty;
    
    public int Order { get; set; }
    
    [Display("Workflow level")]
    public int WorkflowLevel { get; set; }
}

public class Provider 
{
    public string type { get; set; }

    public string id { get; set; }

    public string uid { get; set; }
}
