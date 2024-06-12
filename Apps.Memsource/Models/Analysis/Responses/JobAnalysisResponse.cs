using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Responses
{
    public class JobAnalysisResponse
    {
        [Display("File name")]
        public string Filename { get; set; }

        [Display("Total words")]
        public double TotalWords { get; set; }

        public double Repetitions { get; set; }

        [Display("TM match 100%")]
        public double MemoryMatch100 { get; set; }

        [Display("TM match 95-99%")]
        public double MemoryMatch95 { get; set; }

        [Display("TM match 85-94%")]
        public double MemoryMatch85 { get; set; }

        [Display("TM match 75-84%")]
        public double MemoryMatch75 { get; set; }

        [Display("TM match 50-74%")]
        public double MemoryMatch50 { get; set; }

        [Display("TM no match")]
        public double MemoryMatch0 { get; set; }

        [Display("TM match 101%")]
        public double MemoryMatch101 { get; set; }

        [Display("Total internal fuzzy")]
        public double TotalInternalFuzzy { get; set; }

        [Display("Total machine translation")]
        public double TotalMT { get; set; }

        [Display("Total non translatable")]
        public double TotalNonTranslatable { get; set; }

    }
}
