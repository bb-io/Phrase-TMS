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
        NormalizeForUpdateRequest(payload);

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

    private static void NormalizeForUpdateRequest(JObject payload)
    {
        if (payload["webhook"] is JObject webhook)
        {
            var webhookToken = webhook["webhookToken"]?.Value<string>();
            if (!string.IsNullOrWhiteSpace(webhookToken))
            {
                payload["webhookToken"] = webhookToken;
            }

            payload.Remove("webhook");
        }

        NormalizeEmailTemplateUid(payload["createProjectAutomation"] as JObject);
        NormalizeEmailTemplateUid(payload["sourceUpdateAutomation"] as JObject);
        NormalizeEmailTemplateUid(payload["targetUpdateAutomation"] as JObject);

        payload.Remove("warnings");
        payload.Remove("deprecatedTargetLangs");
        payload.Remove("sourceLang");
    }

    private static void NormalizeEmailTemplateUid(JObject? automation)
    {
        if (automation?["notifyProjectOwnerEmailTemplate"] is not JObject emailTemplate)
            return;

        var uid = emailTemplate["uid"]?.Value<string>();
        if (!string.IsNullOrWhiteSpace(uid))
        {
            automation["notifyProjectOwnerEmailTemplateUid"] = uid;
        }

        automation.Remove("notifyProjectOwnerEmailTemplate");
    }
}
