using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class FindProjectTermbaseRequest
    {

        [JsonProperty("projectUid")]
        [Display("Project ID")]
        [DataSource(typeof(ProjectDataHandler))]
        public string ProjectUId { get; set; }


        [Display("Language code")]
        [JsonProperty("languageCode")]
        [DataSource(typeof(LanguageDataHandler))]
        public string? LanguageCode { get; set; }

        [JsonProperty("name")]
        [Display("Name")]
        public string? Name { get; set; }
    }
}
