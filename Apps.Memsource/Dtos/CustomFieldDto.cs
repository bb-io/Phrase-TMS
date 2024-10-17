using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos
{
    public class CustomFieldDto
    {
        [JsonProperty("uid")]
        public string UId { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


    }

}
