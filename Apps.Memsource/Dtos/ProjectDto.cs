using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class ProjectDto
{
    public string? Name { get; set; }

    [Display("Project ID")] 
    public string UId { get; set; }

    [Display("Creation date")] 
    public DateTime? DateCreated { get; set; }

    [Display("Source language")] 
    public string SourceLang { get; set; }

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

    [Display("Sub domain")]
    [JsonProperty("subDomain")] 
    public SubDomain? SubDomain { get; set; }

    [Display("Owner")]
    [JsonProperty("owner")] 
    public User? Owner { get; set; }

    [Display("Created by")]
    [JsonProperty("createdBy")]
    public User? CreatedBy { get; set; }

    [Display("Shared")]
    public bool Shared { get; set; }

    [Display("Progress")]
    public Progress Progress { get; set; }

    [Display("Puchase order")]
    [JsonProperty("purchaseOrder")]
    public string PurchaseOrder { get; set; }

    [Display("Is published on job board?")]
    [JsonProperty("isPublishedOnJobBoard")]
    public string IsPublishedOnJobBoard { get; set; }

    [DefinitionIgnore]
    [JsonProperty("workflowSteps")]
    public IEnumerable<WorkflowStep> WorkflowSteps { get; set; }
}

public class Domain
{
    [Display("Domain Name")] 
    public string Name { get; set; }
}

public class SubDomain
{
    [Display("Subdomain Name")] 
    public string Name { get; set; }
}

public class Client
{
    [Display("Client Name")] 
    public string Name { get; set; }
}

public class User
{
    [Display("First Name")] 
    public string FirstName { get; set; }

    [Display("Last Name")] 
    public string LastName { get; set; }

    [Display("Username")] 
    public string UserName { get; set; }

    [Display("Email")] 
    public string Email { get; set; }

    [Display("Role")] 
    public string Role { get; set; }

    [Display("User ID")]
    public string Uid { get; set; }
}

public class Progress
{
    [Display("Total count")]
    public double TotalCount { get; set; }

    [Display("Finished count")]
    public double FinishedCount { get; set; }

    [Display("Overdue count")]
    public double OverdueCount { get; set; }
}

public class WorkflowStep
{
    public string Name { get; set; }

    [JsonProperty("workflowStep")]
    public InnerWorkflowStep InnerWorkflowStep { get; set; }

    public int WorkflowLevel { get; set; }
}

public class InnerWorkflowStep
{
    public string Id { get; set; }
}