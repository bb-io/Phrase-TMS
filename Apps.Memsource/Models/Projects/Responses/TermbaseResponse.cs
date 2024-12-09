using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Projects.Responses
{
    public class TermbaseResponse
    {
        [JsonProperty("termBases")]
        [Display("List of term bases")]
        public List<TermbaseWrapper> TermBases { get; set; }
    }

    public class TermbaseWrapper
    {
        [JsonProperty("termBase")]
        [Display("Term base details")]
        public TermbaseDto TermBase { get; set; }

        [JsonProperty("targetLocale")]
        [Display("Target locale")]
        public string TargetLocale { get; set; }

        [JsonProperty("workflowStep")]
        [Display("Workflow step")]
        public WorkflowStepDto WorkflowStep { get; set; }

        [JsonProperty("readMode")]
        [Display("Read mode")]
        public bool ReadMode { get; set; }

        [JsonProperty("writeMode")]
        [Display("Write mode")]
        public bool WriteMode { get; set; }
    }

    public class TermbaseDto
    {
        [JsonProperty("id")]
        [Display("Term base ID")]
        public string Id { get; set; }

        [JsonProperty("name")]
        [Display("Term base name")]
        public string Name { get; set; }

        [JsonProperty("langs")]
        [Display("Supported languages")]
        public List<string> Langs { get; set; }

        [JsonProperty("internalId")]
        [Display("Internal ID")]
        public int InternalId { get; set; }

        [JsonProperty("dateCreated")]
        [Display("Date created")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("note")]
        [Display("Notes")]
        public string Note { get; set; }
    }


    public class WorkflowStepDto
    {
        [JsonProperty("name")]
        [Display("Step mame")]
        public string Name { get; set; }

        [JsonProperty("id")]
        [Display("Step ID")]
        public string Id { get; set; }

        [JsonProperty("uid")]
        [Display("Step UID")]
        public string Uid { get; set; }

        [JsonProperty("order")]
        [Display("Step order")]
        public int Order { get; set; }

        [JsonProperty("lqaEnabled")]
        [Display("LQA enabled")]
        public bool LqaEnabled { get; set; }
    }
}
