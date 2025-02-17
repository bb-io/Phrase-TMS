using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class CreateFromTemplateRequest
{
    public string Name { get; set; }

    [Display("Template UID")]
    [DataSource(typeof(ProjectTemplateDataHandler))]
    public string TemplateUId { get; set; }
    
    [Display("Due date")]
    public DateTime? DateDue { get; set; }


    [Display("Source language")]
    public string? SourceLang { get; set; }


    [Display("Target languages")]
    [DataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string>? TargetLanguages { get; set; }


    [Display("Workflow steps")]
    [DataSource(typeof(WorkflowStepDataHandler))]
    public IEnumerable<string>? WorkflowSteps { get; set; }


    [Display("Note")]
    public string? Note { get; set; }


    [Display("Client Id")]
    public string? ClientId { get; set; }


    [Display("Business unit Id")]
    [DataSource(typeof(BusinessUnitDataHandler))]
    public string? BusinessUnitId { get; set; }


    [Display("Domain Id")]
    [DataSource(typeof(DomainDataHandler))]
    public string? DomainId { get; set; }


    [Display("Sub-domain Id")]
    [DataSource(typeof(SubdomainDataHandler))]
    public string? SubDomainId { get; set; }


    [DataSource(typeof(CostCenterDataHandler))]
    [Display("Cost center Id")]
    public string? CostCenterId { get; set; }
}