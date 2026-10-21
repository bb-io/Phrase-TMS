using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class TermStatusDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("New", "New"),
        new("Approved", "Approved")
    ];
}

public class TermGenderDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("MASCULINE", "Masculine"),
        new("FEMININE", "Feminine"),
        new("NEUTRAL", "Neutral")
    ];
}

public class TermNumberDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("SINGULAR", "Singular"),
        new("PLURAL", "Plural"),
        new("UNCOUNTABLE", "Uncountable")
    ];
}

public class TermPartOfSpeechDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("ADJECTIVE", "Adjective"),
        new("NOUN", "Noun"),
        new("VERB", "Verb"),
        new("ADVERB", "Adverb")
    ];
}

public class TermTypeDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("FULL_FORM", "Full form"),
        new("SHORT_FORM", "Short form"),
        new("ACRONYM", "Acronym"),
        new("ABBREVIATION", "Abbreviation"),
        new("PHRASE", "Phrase"),
        new("VARIANT", "Variant")
    ];
}
