using Apps.PhraseTMS.Helpers;
using Apps.PhraseTMS.Models.AutomatedSettings.Requests;
using Newtonsoft.Json.Linq;

namespace Tests.PhraseTMS;

[TestClass]
public class AutomatedProjectSettingsPayloadBuilderTests
{
    [TestMethod]
    public void Build_PreservesExistingSettingsAndOverridesSelectedFields()
    {
        var existingSettingsJson = """
        {
          "id": "setting-1",
          "name": "UA Text Ads",
          "active": true,
          "selectedTargetLangs": ["de", "fr"],
          "translationExports": [
            {
              "name": "Export 1",
              "exportedWorkflowStepToDeliveredAfterExport": true
            }
          ],
          "monitoredFolders": [
            {
              "remoteFolder": "My Drive/Localization_deliverables/UA Text ads",
              "projectTemplateUid": "template-1"
            }
          ],
          "projectActions": {
            "assignProvidersFromTemplate": false,
            "notifyProjectOwner": true
          },
          "monitorRecurrence": {
            "type": "WEBHOOK"
          }
        }
        """;

        var input = new UpdateAutomatedProjectSettingsRequest
        {
            SelectedTargetLanguages = ["uk"],
            IsActive = false
        };

        var result = JObject.Parse(AutomatedProjectSettingsPayloadBuilder.Build(existingSettingsJson, input));

        Assert.IsNull(result["id"]);
        CollectionAssert.AreEqual(new[] { "uk" }, result["selectedTargetLangs"]!.Values<string>().ToArray());
        Assert.AreEqual(false, result["active"]!.Value<bool>());
        Assert.AreEqual("UA Text Ads", result["name"]!.Value<string>());
        Assert.AreEqual(true, result["projectActions"]!["notifyProjectOwner"]!.Value<bool>());
        Assert.AreEqual("WEBHOOK", result["monitorRecurrence"]!["type"]!.Value<string>());
        Assert.AreEqual(
            "My Drive/Localization_deliverables/UA Text ads",
            result["monitoredFolders"]![0]!["remoteFolder"]!.Value<string>());
    }

    [TestMethod]
    public void Build_KeepsExistingValuesWhenInputsAreEmpty()
    {
        var existingSettingsJson = """
        {
          "id": "setting-1",
          "active": true,
          "selectedTargetLangs": ["de", "fr"],
          "sourceUpdate": {
            "enabled": true
          }
        }
        """;

        var input = new UpdateAutomatedProjectSettingsRequest();

        var result = JObject.Parse(AutomatedProjectSettingsPayloadBuilder.Build(existingSettingsJson, input));

        CollectionAssert.AreEqual(new[] { "de", "fr" }, result["selectedTargetLangs"]!.Values<string>().ToArray());
        Assert.AreEqual(true, result["active"]!.Value<bool>());
        Assert.AreEqual(true, result["sourceUpdate"]!["enabled"]!.Value<bool>());
    }
}
