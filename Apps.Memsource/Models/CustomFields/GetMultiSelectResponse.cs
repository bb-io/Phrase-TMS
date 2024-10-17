using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.CustomFields
{
    public class GetMultiSelectResponse
    {
        [Display("Selected options")]
        public IEnumerable<string> Results { get; set; }
    }
}
