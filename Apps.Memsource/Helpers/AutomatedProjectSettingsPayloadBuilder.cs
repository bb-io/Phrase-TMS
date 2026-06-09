using Apps.PhraseTMS.Models.AutomatedSettings.Requests;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.PhraseTMS.Helpers;

public static class AutomatedProjectSettingsPayloadBuilder
{
    public static string Build(string? existingSettingsJson, UpdateAutomatedProjectSettingsRequest input)
    {
        if (string.IsNullOrWhiteSpace(existingSettingsJson))
            throw new PluginApplicationException("Automated project settings response is empty.");

        var payload = JObject.Parse(existingSettingsJson);

        if (input.SelectedTargetLanguages is not null)
        {
            payload["selectedTargetLangs"] = JArray.FromObject(input.SelectedTargetLanguages);
        }

        if (input.IsActive.HasValue)
        {
            payload["active"] = input.IsActive.Value;
        }

        payload.Remove("id");

        return payload.ToString(Formatting.None);
    }
}
