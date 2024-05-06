using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class ProjectDto
{
    public string? Name { get; set; }

    [Display("UID")] public string UId { get; set; }

    [Display("Creation date")] public DateTime? DateCreated { get; set; }

    [Display("Source language")] public string SourceLang { get; set; }

    [Display("Target languages")] public List<string> TargetLangs { get; set; }

    [Display("Due date")] public DateTime? DateDue { get; set; }

    [Display("Status")] public string Status { get; set; }

    [Display("Note")] public string? Note { get; set; }

    [JsonProperty("domain")] public Domain? Domain { get; set; }

    [JsonProperty("client")] public Client? Client { get; set; }

    [JsonProperty("subDomain")] public SubDomain? SubDomain { get; set; }

    [JsonProperty("owner")] public Owner? Owner { get; set; }
}

public class Domain
{
    [Display("Domain Name")] public string Name { get; set; }

    //public string id { get; set; }

    //public string uid { get; set; }
}

public class SubDomain
{
    [Display("Subdomain Name")] public string Name { get; set; }

    //public string id { get; set; }

    //public string uid { get; set; }
}

public class Client
{
    [Display("Client Name")] public string Name { get; set; }

    //public string id { get; set; }

    //public string uid { get; set; }
}

public class Owner
{
    [Display("Owner First Name")] public string FirstName { get; set; }

    [Display("Owner Last Name")] public string LastName { get; set; }

    [Display("Owner User Name")] public string UserName { get; set; }

    [Display("Owner Email")] public string Email { get; set; }

    [Display("Owner Role")] public string Role { get; set; }

    //public string id { get; set; }

    //public string uid { get; set; }
}