using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class WarningTypesDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
    {
        new DataSourceItem("AbsoluteLength", "Absolute length"),
        new DataSourceItem("CustomQA", "Custom QA"),
        new DataSourceItem("EmptyPairTags", "Empty pair tags"),
        new DataSourceItem("EmptyTagContent", "Empty tag content"),
        new DataSourceItem("EmptyTranslation", "Empty translation"),
        new DataSourceItem("ExtraNumbersV3", "Extra numbers V3"),
        new DataSourceItem("ForbiddenString", "Forbidden string"),
        new DataSourceItem("ForbiddenTerm", "Forbidden term"),
        new DataSourceItem("Formatting", "Formatting"),
        new DataSourceItem("FuzzyInconsistencySourceTarget", "Fuzzy inconsistency source target"),
        new DataSourceItem("FuzzyInconsistencyTargetSource", "Fuzzy inconsistency target source"),
        new DataSourceItem("InconsistentTagContent", "Inconsistent tag content"),
        new DataSourceItem("InconsistentTranslationSourceTarget", "Inconsistent translation source target"),
        new DataSourceItem("InconsistentTranslationTargetSource", "Inconsistent translation target source"),
        new DataSourceItem("JoinTags", "Join tags"),
        new DataSourceItem("LeadingAndTrailingSpaces", "Leading and trailing spaces"),
        new DataSourceItem("LeadingSpaces", "Leading spaces"),
        new DataSourceItem("Malformed", "Malformed"),
        new DataSourceItem("MissingNonTranslatableAnnotation", "Missing non translatable annotation"),
        new DataSourceItem("MissingNumbersV3", "Missing numbers V3"),
        new DataSourceItem("Moravia", "Moravia"),
        new DataSourceItem("MultipleSpacesV3", "Multiple spaces V3"),
        new DataSourceItem("NestedTags", "Nested tags"),
        new DataSourceItem("NewerAtLowerLevel", "Newer at lower level"),
        new DataSourceItem("NonConformingTerm", "Non conforming term"),
        new DataSourceItem("NotConfirmed", "Not confirmed"),
        new DataSourceItem("RelativeLength", "Relative length"),
        new DataSourceItem("RepeatedWord", "Repeated word"),
        new DataSourceItem("SourceOrTargetRegexp", "Source or target regex"),
        new DataSourceItem("SpellCheck", "Spell check"),
        new DataSourceItem("TargetSourceIdentical", "Target source identical"),
        new DataSourceItem("TrailingPunctuation", "Trailing punctuation"),
        new DataSourceItem("TrailingSpaces", "Trailing spaces"),
        new DataSourceItem("TranslationLength", "Translation length"),
        new DataSourceItem("UnmodifiedFuzzyTranslation", "Unmodified fuzzy translation"),
        new DataSourceItem("UnmodifiedFuzzyTranslationMTNT", "Unmodified fuzzy translation MTNT"),
        new DataSourceItem("UnmodifiedFuzzyTranslationTM", "Unmodified fuzzy translation TM"),
        new DataSourceItem("UnresolvedComment", "Unresolved comment"),
        new DataSourceItem("UnresolvedConversation", "Unresolved conversation")
    };
}