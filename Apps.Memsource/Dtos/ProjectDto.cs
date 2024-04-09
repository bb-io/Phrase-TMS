using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class ProjectDto
{
    
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
    public string? Note { get; set; }

    [JsonProperty("domain")]
    public Domain? Domain { get; set; }

    [JsonProperty("client")]
    public Client? Client { get; set; }

    [JsonProperty("subDomain")]
    public subDomain? SubDomain { get; set; }

    [JsonProperty("owner")]
    public owner? Owner { get; set; }

}

public class Domain 
{
    [Display("Domain Name")]
    public string name { get; set; }

    //public string id { get; set; }

    //public string uid { get; set; }
}

public class subDomain
{
    [Display("Subdomain Name")]
    public string name { get; set; }

    //public string id { get; set; }

    //public string uid { get; set; }
}

public class Client
{
    [Display("Client Name")]
    public string name { get; set; }

    //public string id { get; set; }

    //public string uid { get; set; }
}

public class owner
{
    [Display("Owner First Name")]
    public string firstName { get; set; }

    [Display("Owner Last Name")]
    public string lastName { get; set; }

    [Display("Owner User Name")]
    public string userName { get; set; }

    [Display("Owner Email")]
    public string email { get; set; }

    [Display("Owner Role")]
    public string role { get; set; }

    //public string id { get; set; }

    //public string uid { get; set; }
}