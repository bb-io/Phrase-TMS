using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class CreateAnalysisInput : ProjectRequest
{
    [Display("Jobs")]
    [DataSource(typeof(JobDataHandler))]
    public IEnumerable<string> JobsUIds { get; set; }

    public string? Type { get; set; }

    [Display("Include Fuzzy Repetitions")] public bool? IncludeFuzzyRepetitions { get; set; }

    [Display("Separate Fuzzy Repetitions")]
    public bool? SeparateFuzzyRepetitions { get; set; }

    [Display("Include Confirmed Segments")]
    public bool? IncludeConfirmedSegments { get; set; }

    [Display("Include Numbers")] public bool? IncludeNumbers { get; set; }

    [Display("Include Locked Segments")] public bool? IncludeLockedSegments { get; set; }

    [Display("Count Source Units")] public bool? CountSourceUnits { get; set; }

    [Display("Include Trans Memory")] public bool? IncludeTransMemory { get; set; }

    [Display("Include Non-Translatables")] public bool? IncludeNonTranslatables { get; set; }

    [Display("Include Machine Translation Matches")]
    public bool? IncludeMachineTranslationMatches { get; set; }

    [Display("Trans Memory Post Editing")] public bool? TransMemoryPostEditing { get; set; }

    [Display("Non-Translatable Post Editing")]
    public bool? NonTranslatablePostEditing { get; set; }

    [Display("Machine Translate Post Editing")]
    public bool? MachineTranslatePostEditing { get; set; }

    [Display("Name")] public string? Name { get; set; }

    [Display("Compare Workflow Level")] public int? CompareWorkflowLevel { get; set; }

    [Display("Use Project Analysis Settings")]
    public bool? UseProjectAnalysisSettings { get; set; }

    [Display("Callback Url")] public string? CallbackUrl { get; set; }
    [Display("Net rate scheme ID")] public string? NetRateSchemeId { get; set; }
        
    [Display("Provider type")] 
    [DataSource(typeof(ProviderTypeDataHandler))]
    public string? ProviderType { get; set; }
        
    [Display("Provider ID")] public string? ProviderId { get; set; }
    [Display("Default project owner ID")] public int? DefaultProjectOwnerId { get; set; }
}