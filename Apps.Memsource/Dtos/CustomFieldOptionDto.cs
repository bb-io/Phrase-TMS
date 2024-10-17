using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos
{
    public class CustomFieldOptionDto
    {
        [JsonProperty("uid")]
        public string UId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

}
