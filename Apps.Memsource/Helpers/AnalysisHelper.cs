using Blackbird.Filters.Analysis.Models;
using Newtonsoft.Json.Linq;

namespace Apps.PhraseTMS.Helpers;

public static class AnalysisHelper
{
    private static readonly Dictionary<string, AnalysisType> AnalysisMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "contextMatch.words", AnalysisType.ExactMatch },
        { "match100.words.sum", AnalysisType.ExactMatch },
        { "repetitions.words", AnalysisType.Repetition },
        { "match95.words.sum", AnalysisType.Match9599 },
        { "match85.words.sum", AnalysisType.Match8594 },
        { "match75.words.sum", AnalysisType.Match7584 },
        { "match50.words.sum", AnalysisType.Match5074 },
        { "match0.words.sum", AnalysisType.NoMatch }
    };

    public static List<Analysis> GenerateAnalysis(JArray analyseLanguageParts)
    {
        List<Analysis> results = [];

        foreach (JToken langPart in analyseLanguageParts)
        {
            if (langPart is not JObject langObj) 
                continue;
            
            string originalLocale = langObj["targetLang"]?.ToString() ?? "unknown";
            string normalizedLocale = originalLocale.Replace('_', '-');

            if (normalizedLocale.Split('-').Length > 1)
            {
                string[] splitLocale = normalizedLocale.Split('-'); 
                string localeCode = splitLocale.Last().ToUpper();
                normalizedLocale = splitLocale[0] + '-' + localeCode;
            }
            
            if (langObj["data"] is JObject dataNode)
            {
                var analysisRecord = Analysis.Map(normalizedLocale, originalLocale, dataNode, AnalysisMap);
                results.Add(analysisRecord);
            }
        }

        return results;
    }
}