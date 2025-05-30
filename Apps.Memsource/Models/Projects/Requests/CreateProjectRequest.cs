﻿using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class CreateProjectRequest
{
    public string Name { get; set; }

    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLanguage { get; set; }


    [Display("Target languages")]
    [DataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string> TargetLanguages { get; set; }
    

    [Display("Due date")]
    public DateTime? DateDue { get; set; }


    [Display("Client ID")]
    [DataSource(typeof(ClientDataHandler))]
    public string? ClientId { get; set; }


    [Display("Business unit ID")]
    [DataSource(typeof(BusinessUnitDataHandler))]
    public string? BusinessUnitId { get; set; }


    [Display("Domain ID")]
    [DataSource(typeof(DomainDataHandler))]
    public string? DomainId { get; set; }


    [Display("Sub-domain ID")]
    [DataSource(typeof(SubdomainDataHandler))]
    public string? SubDomainId { get; set; }


    [DataSource(typeof(CostCenterDataHandler))]
    [Display("Cost center ID")]
    public string? CostCenterId { get; set; }


    [Display("Purchase order")]
    public string? PurchaseOrder { get; set; }


    [Display("Workflow steps")]
    [DataSource(typeof(WorkflowStepDataHandler))]
    public IEnumerable<string>? WorkflowSteps { get; set; }


    [Display("Note")]
    public string? Note { get; set; }


    [Display("File handover")]
    public bool? FileHandover { get; set; }


    [Display("Propagate translations")]
    public bool? PropagateTranslationsToLowerWfDuringUpdateSource { get; set; }
}