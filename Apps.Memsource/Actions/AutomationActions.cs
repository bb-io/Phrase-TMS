using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Dtos.AutomatedSettings;
using Apps.PhraseTMS.Models.AutomatedSettings.Requests;
using Apps.PhraseTMS.Models.AutomatedSettings.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.PhraseTMS.Actions;

[ActionList("Automations")]
public class AutomationActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    //https://developers.phrase.com/en/api/tms/latest/automations/update-automated-project-settings
    [Action("Update automated project settings", Description = "Update specific automated project settings")]
    public async Task<UpdateAutomatedProjectSettingsResponse> UpdateAutomatedProjectSettings(
        [ActionParameter] AutomatedProjectSettingRequest settingRequest,
        [ActionParameter] UpdateAutomatedProjectSettingsRequest input)
    {
        var existingSettingsRequest = new RestRequest($"api2/v3/automatedProjects/{settingRequest.SettingId}");
        var existingSettingsResponse =
            await Client.ExecuteWithHandling<AutomatedProjectSettingDto>(existingSettingsRequest);

        var body = new Dictionary<string, object>
        {
            { "name", existingSettingsResponse.Name },
            { "monitoredFolders", existingSettingsResponse.MonitoredFolders },
            { "translationExports", existingSettingsResponse.TranslationExports },
            { "selectedTargetLangs", input.SelectedTargetLanguages ?? existingSettingsResponse.SelectedTargetLanguages },
            { "active", input.IsActive ?? existingSettingsResponse.IsActive }
        };
        
        var updateRequest = new RestRequest($"api2/v3/automatedProjects/{settingRequest.SettingId}", Method.Put)
            .WithJsonBody(body, JsonConfig.Settings);
        var updateResponse = await Client.ExecuteWithHandling<AutomatedProjectSettingDto>(updateRequest);

        return new(updateResponse);
    }
}