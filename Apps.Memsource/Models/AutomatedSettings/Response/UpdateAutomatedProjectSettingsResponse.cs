using Apps.PhraseTMS.Dtos.AutomatedSettings;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.AutomatedSettings.Response;

public record UpdateAutomatedProjectSettingsResponse(AutomatedProjectSettingDto dto)
{
    [Display("Setting ID")] 
    public string Id { get; set; } = dto.Id;

    [Display("Setting name")]
    public string Name { get; set; } = dto.Name;

    [Display("Is active")]
    public bool IsActive { get; set; } = dto.IsActive;

    [Display("Selected target languages")]
    public IEnumerable<string> SelectedTargetLanguages { get; set; } = dto.SelectedTargetLanguages;
}