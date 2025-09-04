using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.TranslationMemories.Responses
{
    public class EditProjectTransMemoriesResponse
    {
        [Display("Translation memories")]
        public List<ProjectTransMemoryAssignmentDto>? TransMemories { get; set; }
    }

    public class ProjectTransMemoryAssignmentDto
    {
        [Display("Translation memory")]
        public TranslationMemoryDto? TransMemory { get; set; }
        public decimal? Penalty { get; set; }

        [Display("Apply penalty to 101 only")]
        public bool? ApplyPenaltyTo101Only { get; set; }

        [Display("Target locale")]
        public string? TargetLocale { get; set; }

        [Display("Workflow step")]
        public WorkflowStepDto? WorkflowStep { get; set; }

        [Display("Read mode")]
        public bool? ReadMode { get; set; }

        [Display("Write mode")]
        public bool? WriteMode { get; set; }

        [Display("Order")]
        public int? Order { get; set; }
    }

    public class WorkflowStepDto
    {
        public string? Name { get; set; }
        public string? Id { get; set; }
        public string? Uid { get; set; }
        public int? Order { get; set; }

        [Display("LQA enabled")]
        public bool? LqaEnabled { get; set; }
    }
}
