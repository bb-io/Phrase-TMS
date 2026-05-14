using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.AutomatedSettings.Requests;

public class UpdateAutomatedProjectSettingsRequest
{
    [Display("Selected target languages", Description = "Overwrites target languages for the specified setting")] 
    [DataSource(typeof(LanguageDataHandler))] 
    public IEnumerable<string> SelectedTargetLanguages { get; set; } = [];

    [Display("Is active")]
    public bool? IsActive { get; set; }
}