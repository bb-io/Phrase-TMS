namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class CreateAnalysisRequest
{
    public CreateAnalysisRequest()
    {
    }

    public CreateAnalysisRequest(CreateAnalysisInput input)
    {
        Jobs = input.JobsUIds.Select(x => new JobUidModel(x)).ToArray();
        Type = input.Type;
        IncludeFuzzyRepetitions = input.IncludeFuzzyRepetitions;
        SeparateFuzzyRepetitions = input.SeparateFuzzyRepetitions;
        IncludeConfirmedSegments = input.IncludeConfirmedSegments;
        IncludeNumbers = input.IncludeNumbers;
        IncludeLockedSegments = input.IncludeLockedSegments;
        CountSourceUnits = input.CountSourceUnits;
        IncludeTransMemory = input.IncludeTransMemory;
        IncludeNonTranslatables = input.IncludeNonTranslatables;
        IncludeMachineTranslationMatches = input.IncludeMachineTranslationMatches;
        TransMemoryPostEditing = input.TransMemoryPostEditing;
        NonTranslatablePostEditing = input.NonTranslatablePostEditing;
        MachineTranslatePostEditing = input.MachineTranslatePostEditing;
        Name = input.Name;
        CompareWorkflowLevel = input.CompareWorkflowLevel;
        UseProjectAnalysisSettings = input.UseProjectAnalysisSettings;
        CallbackUrl = input.CallbackUrl;
        NetRateScheme = input.NetRateSchemeId is null ? null : new() { Id = input.NetRateSchemeId };
        Provider = input.ProviderId is null
            ? null
            : new()
            {
                Id = input.ProviderId,
                Type = input.ProviderType,
                DefaultProjectOwnerId = input.DefaultProjectOwnerId
            };
    }

    public IEnumerable<JobUidModel> Jobs { get; set; }
    public string? Type { get; set; }

    public bool? IncludeFuzzyRepetitions { get; set; }

    public bool? SeparateFuzzyRepetitions { get; set; }

    public bool? IncludeConfirmedSegments { get; set; }

    public bool? IncludeNumbers { get; set; }

    public bool? IncludeLockedSegments { get; set; }

    public bool? CountSourceUnits { get; set; }

    public bool? IncludeTransMemory { get; set; }

    public bool? IncludeNonTranslatables { get; set; }


    public bool? IncludeMachineTranslationMatches { get; set; }

    public bool? TransMemoryPostEditing { get; set; }

    public bool? NonTranslatablePostEditing { get; set; }

    public bool? MachineTranslatePostEditing { get; set; }

    public string? Name { get; set; }

    public int? CompareWorkflowLevel { get; set; }

    public bool? UseProjectAnalysisSettings { get; set; }

    public string? CallbackUrl { get; set; }
    public NetRateScheme? NetRateScheme { get; set; }
    public Provider? Provider { get; set; }
}