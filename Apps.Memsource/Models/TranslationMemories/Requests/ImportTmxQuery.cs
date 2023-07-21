using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class ImportTmxQuery
{
    [Display("Strict language matching")] public bool? StrictLangMatching { get; set; }

    [Display("Strip native codes")] public bool? StripNativeCodes { get; set; }
}