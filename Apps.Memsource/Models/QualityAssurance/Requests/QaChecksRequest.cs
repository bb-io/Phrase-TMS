using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests
{
    public class QaChecksRequest
    {
        [Display("QA warning types")]
        [StaticDataSource(typeof(WarningTypesDataHandler))]
        public IEnumerable<string>? WarningTypes { get; set; }
    }
}
