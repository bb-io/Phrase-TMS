using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos.Jobs;
public class WorkflowStepDto
{
    [Display("Workflow step ID")]
    [JsonProperty("id")]
    public string Id { get; set; }

    [Display("Name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [Display("Order")]
    [JsonProperty("order")]
    public int Order { get; set; }

    [Display("Workflow level")]
    [JsonProperty("workflowLevel")]
    public int WorkflowLevel { get; set; }
}
