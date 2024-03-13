using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class ProjectDto
{
    public string Id { get; set; }
    public string? Name { get; set; }

    [Display("UID")]
    public string UId { get; set; }

    [Display("Creation date")]
    public DateTime? DateCreated { get; set; }

    [Display("Source language")]
    public string sourceLang { get; set; }

    [Display("Target languages")]
    public List<string> TargetLangs { get; set; }

    [Display("Due date")]
    public DateTime? DateDue { get; set; }

    [Display("Status")]
    public string Status { get; set; }

    [Display("Note")]
    public string Note { get; set; }

    [JsonProperty("domain")]
    public Domain Domain { get; set; }

    [JsonProperty("client")]
    public Client Client { get; set; }

    [JsonProperty("subDomain")]
    public subDomain SubDomain { get; set; }

    [JsonProperty("owner")]
    public owner Owner { get; set; }

}

public class Domain 
{
    public string name { get; set; }

    public string id { get; set; }

    public string uid { get; set; }
}

public class subDomain
{
    public string name { get; set; }

    public string id { get; set; }

    public string uid { get; set; }
}

public class Client
{
    public string name { get; set; }

    public string id { get; set; }

    public string uid { get; set; }
}

public class owner
{
    public string firstName { get; set; }

    public string lastName { get; set; }

    public string userName { get; set; }

    public string email { get; set; }

    public string role { get; set; }

    public string id { get; set; }

    public string uid { get; set; }
}