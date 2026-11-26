using Apps.PhraseTMS.Dtos.Analysis;
using Newtonsoft.Json;

namespace Tests.PhraseTMS;

[TestClass]
public class AnalysisDataDeserializationTests
{
    private const string MatchJson = "{\"editingTime\":0,\"percent\":3.3,\"words\":36,\"segments\":19,\"characters\":200,\"normalizedPages\":0.1}";

    private static readonly string MatchesJson =
        $"{{\"match0\":{MatchJson},\"match50\":{MatchJson},\"match75\":{MatchJson},\"match85\":{MatchJson},\"match95\":{MatchJson},\"match100\":{MatchJson}}}";

    private static readonly string DataJson =
        $"{{\"available\":true,\"estimate\":false,\"all\":{MatchJson},\"repetitions\":{MatchJson}," +
        $"\"internalFuzzyMatches\":{MatchesJson},\"machineTranslationMatches\":{MatchesJson}," +
        $"\"transMemoryMatches\":{MatchesJson},\"nonTranslatablesMatches\":{MatchesJson}}}";

    [TestMethod]
    public void Language_part_without_discounted_data_exposes_zero_discounted_counts()
    {
        var analysis = DeserializeAnalysisWithLanguagePart($"\"data\":{DataJson},\"discountedData\":null");

        var languagePart = analysis.AnalyseLanguageParts.Single();
        Assert.IsFalse(languagePart.HasDiscountedData);
        Assert.IsNotNull(languagePart.DiscountedData);
        Assert.AreEqual(0, languagePart.DiscountedData.NonTranslatablesMatches.Match95.Words);
        Assert.AreEqual(0, languagePart.DiscountedData.TransMemoryMatches.Match100.Words);
        Assert.AreEqual(0, languagePart.DiscountedData.All.Words);
        Assert.AreEqual(36, languagePart.Data.NonTranslatablesMatches.Match95.Words);
    }

    [TestMethod]
    public void Language_part_with_missing_discounted_data_exposes_zero_discounted_counts()
    {
        var analysis = DeserializeAnalysisWithLanguagePart($"\"data\":{DataJson}");

        var languagePart = analysis.AnalyseLanguageParts.Single();
        Assert.IsFalse(languagePart.HasDiscountedData);
        Assert.AreEqual(0, languagePart.DiscountedData.MachineTranslationMatches.Match0.Words);
    }

    [TestMethod]
    public void Language_part_with_discounted_data_keeps_its_values()
    {
        var analysis = DeserializeAnalysisWithLanguagePart($"\"data\":{DataJson},\"discountedData\":{DataJson}");

        var languagePart = analysis.AnalyseLanguageParts.Single();
        Assert.IsTrue(languagePart.HasDiscountedData);
        Assert.AreEqual(36, languagePart.DiscountedData.NonTranslatablesMatches.Match95.Words);
    }

    [TestMethod]
    public void Language_part_with_null_match_band_exposes_zero_counts_for_it()
    {
        var dataWithNullBands = DataJson
            .Replace($"\"nonTranslatablesMatches\":{MatchesJson}", "\"nonTranslatablesMatches\":{\"match95\":null}")
            .Replace($"\"repetitions\":{MatchJson}", "\"repetitions\":null");

        var analysis = DeserializeAnalysisWithLanguagePart($"\"data\":{dataWithNullBands},\"discountedData\":null");

        var data = analysis.AnalyseLanguageParts.Single().Data;
        Assert.AreEqual(0, data.NonTranslatablesMatches.Match95.Words);
        Assert.AreEqual(0, data.NonTranslatablesMatches.Match100.Words);
        Assert.AreEqual(0, data.Repetitions.Words);
        Assert.AreEqual(36, data.TransMemoryMatches.Match95.Words);
    }

    private static FullAnalysisDto DeserializeAnalysisWithLanguagePart(string languagePartBody)
    {
        var json = $"{{\"uid\":\"analysis-1\",\"analyseLanguageParts\":[{{\"id\":\"part-1\",\"sourceLang\":\"en_us\",\"targetLang\":\"fi_fi\",{languagePartBody}}}]}}";
        return JsonConvert.DeserializeObject<FullAnalysisDto>(json)!;
    }
}
