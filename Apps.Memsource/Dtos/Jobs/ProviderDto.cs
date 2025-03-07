using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos.Jobs;
public class ProviderDto
{
    [Display("Provider ID")]
    [JsonProperty("uid")]
    public string uid { get; set; }

    [Display("Type")]
    [JsonProperty("type")]
    public string type { get; set; }

}
