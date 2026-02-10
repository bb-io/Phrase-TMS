using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Responses;

public class GetProjectResponse : ProjectDto
{
    [Display("Workflow step names")]
    public IEnumerable<string>? WorkflowStepNames { get; set; }

    public GetProjectResponse(ProjectDto dto)
    {
        Name = dto.Name;
        UId = dto.UId;
        DateCreated = dto.DateCreated;
        SourceLang = dto.SourceLang;
        TargetLangs = dto.TargetLangs;
        DateDue = dto.DateDue;
        Status = dto.Status;
        Note = dto.Note;
        Domain = dto.Domain;
        Client = dto.Client;
        SubDomain = dto.SubDomain;
        Owner = dto.Owner;
        CreatedBy = dto.CreatedBy;
        Shared = dto.Shared;
        Progress = dto.Progress;
        PurchaseOrder = dto.PurchaseOrder;
        IsPublishedOnJobBoard = dto.IsPublishedOnJobBoard;
        WorkflowSteps = dto.WorkflowSteps;
        BuyerOwner = dto.BuyerOwner;
        Buyer = dto.Buyer;

        WorkflowStepNames = dto.WorkflowSteps?
            .Select(ws => ws.Name)
            .Where(n => !string.IsNullOrWhiteSpace(n));
    }
}
