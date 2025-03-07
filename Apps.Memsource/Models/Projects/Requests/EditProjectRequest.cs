using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class EditProjectRequest
{
    [Display("Project name")] public string ProjectName { get; set; }

    [StaticDataSource(typeof(ProjectStatusDataHandler))]
    public string Status { get; set; }
    
    [Display("Due date")]
    public DateTime? DueDate { get; set; }

    [Display("Domain ID"), DataSource(typeof(DomainDataHandler))]
    public string? DomainId { get; set; }
    
    [Display("Subdomain ID"), DataSource(typeof(SubdomainDataHandler))]
    public string? SubdomainId { get; set; }

    [Display("Client ID"), DataSource(typeof(ClientDataHandler))]
    public string? ClientId { get; set; }
    
    [Display("Business unit"), DataSource(typeof(BusinessUnitDataHandler))]
    public string? BusinessUnit { get; set; }

    [Display("Owner user ID"), DataSource(typeof(UserDataHandler))]
    public string? OwnerId { get; set; }
}