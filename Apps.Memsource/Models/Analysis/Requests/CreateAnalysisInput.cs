using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class CreateAnalysisInput
{
    [Display("Job IDs")]
    [DataSource(typeof(JobDataHandler))]
    public IEnumerable<string> JobsUIds { get; set; }

    public string? Type { get; set; }

    [Display("Include fuzzy repetitions")] public bool? IncludeFuzzyRepetitions { get; set; }

    [Display("Separate fuzzy repetitions")]
    public bool? SeparateFuzzyRepetitions { get; set; }

    [Display("Include confirmed segments")]
    public bool? IncludeConfirmedSegments { get; set; }

    [Display("Include numbers")] public bool? IncludeNumbers { get; set; }

    [Display("Include locked segments")] public bool? IncludeLockedSegments { get; set; }

    [Display("Count source units")] public bool? CountSourceUnits { get; set; }

    [Display("Include trans memory")] public bool? IncludeTransMemory { get; set; }

    [Display("Include non-translatables")] public bool? IncludeNonTranslatables { get; set; }

    [Display("Include machine translation matches")]
    public bool? IncludeMachineTranslationMatches { get; set; }

    [Display("Trans memory post editing")] public bool? TransMemoryPostEditing { get; set; }

    [Display("Non-translatable post editing")]
    public bool? NonTranslatablePostEditing { get; set; }

    [Display("Machine translate post editing")]
    public bool? MachineTranslatePostEditing { get; set; }

    [Display("Name")] public string? Name { get; set; }

    [Display("Compare workflow level")] public int? CompareWorkflowLevel { get; set; }

    [Display("Use project analysis settings")]
    public bool? UseProjectAnalysisSettings { get; set; }

    [Display("Callback url")] public string? CallbackUrl { get; set; }
    
    [Display("Net rate scheme ID")] 
    [DataSource(typeof(NetRateSchemeDataHandler))]
    public string? NetRateSchemeId { get; set; }
        
    [Display("Provider type")] 
    [StaticDataSource(typeof(ProviderTypeDataHandler))]
    public string? ProviderType { get; set; }
        
    [Display("Provider ID")] public string? ProviderId { get; set; }
    [Display("Default project owner ID")] public int? DefaultProjectOwnerId { get; set; }
}