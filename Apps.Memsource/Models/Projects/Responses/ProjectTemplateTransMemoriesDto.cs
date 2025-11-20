namespace Apps.PhraseTMS.Models.Projects.Responses
{
    public class ProjectTemplateTransMemoriesDto
    {
        public IEnumerable<ProjectTemplateTmsPerTargetLangDto>? PttmsPerTargetLang { get; set; }
        public int? MaxTmsEnabled { get; set; }
    }
    public class ProjectTemplateTmsPerTargetLangDto
    {
        public string? TargetLang { get; set; }
        public IEnumerable<ProjectTemplateTmsPerWfStepDto>? PttmsPerWfStep { get; set; }
    }

    public class ProjectTemplateTmsPerWfStepDto
    {
        public ProjectTemplateWorkflowStepRefDto? WfStep { get; set; }
        public IEnumerable<ProjectTemplateTmDataDto>? PttmData { get; set; }
    }

    public class ProjectTemplateWorkflowStepRefDto
    {
        public string? Uid { get; set; }
    }
    public class ProjectTemplateTmDataDto
    {
        public bool ReadMode { get; set; }
        public bool WriteMode { get; set; }
        public int? Penalty { get; set; }
        public bool? ApplyPenaltyTo101Only { get; set; }
        public int? Order { get; set; }
        public string? TargetLang { get; set; }
        public ProjectTemplateTmRefDto? TransMemory { get; set; }
    }

    public class ProjectTemplateTmRefDto
    {
        public string? Uid { get; set; }
    }
}
