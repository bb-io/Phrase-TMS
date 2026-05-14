using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.AutomatedSettings.Requests;

public class AutomatedProjectSettingRequest
{
    [Display("Setting ID"), DataSource(typeof(AutomatedProjectSettingsDataHandler))]
    public string SettingId { get; set; } = string.Empty;
}