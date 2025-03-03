using Apps.PhraseTMS.Dtos.Analysis;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos.Jobs;
public class ListJobDto
{
    [Display("Job ID")]
    [JsonProperty("uid")]
    public string Uid { get; set; }

    [Display("Inner ID", Description = "InnerId is a sequential number of a job in a project.\nJobs created from the same file share the same innerId across workflow steps.")]
    [JsonProperty("innerId")]
    public string InnerId { get; set; }

    [Display("Status")]
    [JsonProperty("status")]
    public string Status { get; set; }

    [Display("Providers")]
    [JsonProperty("providers")]
    public IEnumerable<ProviderDto> Providers { get; set; }

    [Display("Target language")]
    [JsonProperty("targetLang")]
    public string TargetLang { get; set; }

    [Display("Workflow step")]
    [JsonProperty("workflowStep")]
    public WorkflowStepDto WorkflowStep { get; set; }

    [Display("File name")]
    [JsonProperty("filename")]
    public string Filename { get; set; }

    [Display("Created date")]
    [JsonProperty("dateCreated")]
    public DateTime DateCreated { get; set; }

    [Display("Due date")]
    [JsonProperty("dateDue")]
    public DateTime? DateDue { get; set; }

    [Display("Continuous")]
    [JsonProperty("continuous")]
    public bool Continuous { get; set; }

    [Display("Source file ID")]
    [JsonProperty("sourceFileUid")]
    public string SourceFileUid { get; set; }

    [Display("Split")]
    [JsonProperty("split")]
    public bool Split { get; set; }

    [Display("Server task ID")]
    [JsonProperty("serverTaskId")]
    public string ServerTaskId { get; set; }

    [Display("Imported")]
    [JsonProperty("imported")]
    public bool Imported { get; set; }

    [Display("Owner")]
    [JsonProperty("owner")]
    public MinimalUserDto Owner { get; set; }

}
