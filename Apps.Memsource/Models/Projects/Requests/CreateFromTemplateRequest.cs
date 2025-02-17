using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class CreateFromTemplateRequest
{
    public string Name { get; set; }

    [Display("Template UID")]
    [DataSource(typeof(ProjectTemplateDataHandler))]
    public string TemplateUID { get; set; }
    
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


    [Display("Client ID")]
    public string? ClientID { get; set; }


    [Display("Business unit ID")]
    [DataSource(typeof(BusinessUnitDataHandler))]
    public string? BusinessUnitID { get; set; }


    [Display("Domain ID")]
    [DataSource(typeof(DomainDataHandler))]
    public string? DomainID { get; set; }


    [Display("Sub-domain ID")]
    [DataSource(typeof(SubdomainDataHandler))]
    public string? SubDomainID { get; set; }


    [DataSource(typeof(CostCenterDataHandler))]
    [Display("Cost center ID")]
    public string? CostCenterID { get; set; }
}