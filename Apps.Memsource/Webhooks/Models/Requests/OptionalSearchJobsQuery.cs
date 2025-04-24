using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;
public class OptionalSearchJobsQuery
{
    [DefinitionIgnore, JsonProperty("workflowLevel")]
    public int? WorkflowLevel { get; set; }

    [Display("Target language"), JsonProperty("targetLang")]
    [DataSource(typeof(LanguageDataHandler))]
    public string? TargetLang { get; set; }
}
