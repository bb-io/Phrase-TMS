using Blackbird.Applications.Sdk.Common;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Analysis;
public class FullAnalysisDto
{
    [Display("Settings")]
    [JsonProperty("settings")]
    public Settings Settings { get; set; }

    [Display("Import status")]
    [JsonProperty("importStatus")]
    public ImportStatus ImportStatus { get; set; }

    [Display("Type")]
    [JsonProperty("type")]
    public string Type { get; set; }

    [Display("Analysis ID")]
    [JsonProperty("uid")]
    public string Uid { get; set; }

    [Display("Can change net rate scheme")]
    [JsonProperty("canChangeNetRateScheme")]
    public bool CanChangeNetRateScheme { get; set; }

    [Display("Creation date")]
    [JsonProperty("dateCreated")]
    public DateTime DateCreated { get; set; }

    [Display("Language parts")]
    [JsonProperty("analyseLanguageParts")]
    public List<AnalyseLanguagePart> AnalyseLanguageParts { get; set; }

    [Display("Outdated")]
    [JsonProperty("outdated")]
    public bool Outdated { get; set; }

    [Display("Name")]
    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AnalyseLanguagePart
{
    [Display("Jobs")]
    [JsonProperty("jobs")]
    public List<Job> Jobs { get; set; }

    [Display("Language part ID")]
    [JsonProperty("id")]
    public string Id { get; set; }

    [Display("Source language")]
    [JsonProperty("sourceLang")]
    public string SourceLang { get; set; }

    [Display("Target language")]
    [JsonProperty("targetLang")]
    public string TargetLang { get; set; }

    [Display("Data")]
    [JsonProperty("data")]
    public Data Data { get; set; }

    [Display("Discounted data")]
    [JsonProperty("discountedData")]
    public Data? DiscountedData { get; set; }

    [Display("Has discounted data?")]
    public bool HasDiscountedData => DiscountedData != null;

}

public class Data
{
    [Display("Available")]
    [JsonProperty("available")]
    public bool Available { get; set; }

    [Display("Estimate")]
    [JsonProperty("estimate")]
    public bool Estimate { get; set; }

    [Display("All")]
    [JsonProperty("all")]
    public Match All { get; set; }

    [Display("Repetitions")]
    [JsonProperty("repetitions")]
    public Match Repetitions { get; set; }

    [Display("Internal fuzzy matches")]
    [JsonProperty("internalFuzzyMatches")]
    public Matches InternalFuzzyMatches { get; set; }

    [Display("Machine translation matches")]
    [JsonProperty("machineTranslationMatches")]
    public Matches MachineTranslationMatches { get; set; }

    [Display("Translation memory matches")]
    [JsonProperty("transMemoryMatches")]
    public Matches TransMemoryMatches { get; set; }

    [Display("Non translatable matches")]
    [JsonProperty("nonTranslatablesMatches")]
    public Matches NonTranslatablesMatches { get; set; }
}

public class ImportStatus
{
    [Display("Status")]
    [JsonProperty("status")]
    public string Status { get; set; }
}

public class Matches
{
    [Display("Match 0")]
    [JsonProperty("match0")]
    public Match Match0 { get; set; }

    [Display("Match 50")]
    [JsonProperty("match50")]
    public Match Match50 { get; set; }

    [Display("Match 75")]
    [JsonProperty("match75")]
    public Match Match75 { get; set; }

    [Display("Match 85")]
    [JsonProperty("match85")]
    public Match Match85 { get; set; }

    [Display("Match 95")]
    [JsonProperty("match95")]
    public Match Match95 { get; set; }

    [Display("Match 100")]
    [JsonProperty("match100")]
    public Match Match100 { get; set; }
}

public class Job
{
    [Display("Job ID")] 
    [JsonProperty("uid")]
    public string Uid { get; set; }

    [Display("File name")]
    [JsonProperty("filename")]
    public string Filename { get; set; }
}

public class Match
{
    [Display("Editing time")]
    [JsonProperty("editingTime")]
    public double EditingTime { get; set; }

    [Display("Percent")]
    [JsonProperty("percent")]
    public double Percent { get; set; }

    [Display("Words")]
    [JsonProperty("words")]
    public double Words { get; set; }

    [Display("Segments")]
    [JsonProperty("segments")]
    public double Segments { get; set; }

    [Display("Characters")]
    [JsonProperty("characters")]
    public double Characters { get; set; }

    [Display("Normalized pages")]
    [JsonProperty("normalizedPages")]
    public double NormalizedPages { get; set; }
}

public class Project
{
    [Display("Project ID")]
    [JsonProperty("uid")]
    public string Uid { get; set; }

    [Display("Name")]
    [JsonProperty("name")]
    public string Name { get; set; }
}

public class Settings
{
    [Display("Include numbers")]
    [JsonProperty("includeNumbers")]
    public bool IncludeNumbers { get; set; }

    [Display("Include non translatables")]
    [JsonProperty("includeNonTranslatables")]
    public bool IncludeNonTranslatables { get; set; }

    [Display("Include fuzzy repetitions")]
    [JsonProperty("includeFuzzyRepetitions")]
    public bool IncludeFuzzyRepetitions { get; set; }

    [Display("Type")]
    [JsonProperty("type")]
    public string Type { get; set; }

    [Display("Naming pattern")]
    [JsonProperty("namingPattern")]
    public string NamingPattern { get; set; }

    [Display("Include confirmed segments")]
    [JsonProperty("includeConfirmedSegments")]
    public bool IncludeConfirmedSegments { get; set; }

    [Display("Count source units")]
    [JsonProperty("countSourceUnits")]
    public bool CountSourceUnits { get; set; }

    [Display("Analyze by language")]
    [JsonProperty("analyzeByLanguage")]
    public bool AnalyzeByLanguage { get; set; }

    [Display("Analyze by provider")]
    [JsonProperty("analyzeByProvider")]
    public bool AnalyzeByProvider { get; set; }

    [Display("Include machine translations")]
    [JsonProperty("includeMachineTranslationMatches")]
    public bool IncludeMachineTranslationMatches { get; set; }

    [Display("Include locked segments")]
    [JsonProperty("includeLockedSegments")]
    public bool IncludeLockedSegments { get; set; }

    [Display("Separate fuzzy repetitions")]
    [JsonProperty("separateFuzzyRepetitions")]
    public bool SeparateFuzzyRepetitions { get; set; }

    [Display("Include translation memory")]
    [JsonProperty("includeTransMemory")]
    public bool IncludeTransMemory { get; set; }

    [Display("Allow automatic post analysis")]
    [JsonProperty("allowAutomaticPostAnalysis")]
    public bool AllowAutomaticPostAnalysis { get; set; }
}
