using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Requests
{
    public class EditAnalysisRequest
    {
        [Display("Name")] public string? Name { get; set; }

        [Display("Net rate scheme ID")]
        [DataSource(typeof(NetRateSchemeDataHandler))]
        public string? NetRateSchemeId { get; set; }

        [Display("Vendor ID (provider)")]
        [DataSource(typeof(VendorDataHandler))]
        public string? vendorId { get; set; }

        [Display("User ID (provider)")]
        [DataSource(typeof(UserDataHandler))]
        public string? userId { get; set; }
        [Display("Default project owner ID")] public int? DefaultProjectOwnerId { get; set; }
    }
}
