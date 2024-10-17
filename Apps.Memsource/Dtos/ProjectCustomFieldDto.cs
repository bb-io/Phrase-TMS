using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos
{
    public class ProjectCustomFieldDto
    {
        [JsonProperty("uid")]
        public string UId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("customField")]
        public _CustomField customField { get; set; }

        [JsonProperty("selectedOptions")]
        public List<SelectedOption> selectedOptions { get; set; }
    }

    public class SelectedOption
    {
        public string value { get; set; }
        public string uid { get; set; }
    }

    public class _CustomField
    {
        public string uid { get; set; }
        public string type { get; set; }

        public string name { get; set; }
    }
}
