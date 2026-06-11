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

    [TestMethod]
    public void Build_MapsWebhookObjectToWebhookToken()
    {
        var existingSettingsJson = """
        {
          "id": "setting-1",
          "selectedTargetLangs": ["de"],
          "webhook": {
            "webhookUrl": "https://cloud.memsource.com/web/api2/v1/automatedProjects/webhooks/token-123",
            "webhookToken": "token-123"
          }
        }
        """;

        var input = new UpdateAutomatedProjectSettingsRequest
        {
            SelectedTargetLanguages = ["uk"]
        };

        var result = JObject.Parse(AutomatedProjectSettingsPayloadBuilder.Build(existingSettingsJson, input));

        Assert.IsNull(result["webhook"]);
        Assert.AreEqual("token-123", result["webhookToken"]!.Value<string>());
        CollectionAssert.AreEqual(new[] { "uk" }, result["selectedTargetLangs"]!.Values<string>().ToArray());
    }

    [TestMethod]
    public void Build_MapsEmailTemplateObjectsToUidFields()
    {
        var existingSettingsJson = """
        {
          "id": "setting-1",
          "selectedTargetLangs": ["de"],
          "createProjectAutomation": {
            "notifyProjectOwnerEnabled": true,
            "notifyProjectOwnerEmailTemplate": {
              "uid": "template-create",
              "name": "Create template"
            }
          },
          "sourceUpdateAutomation": {
            "notifyProjectOwnerEnabled": true,
            "notifyProjectOwnerEmailTemplate": {
              "uid": "template-source",
              "name": "Source template"
            }
          },
          "targetUpdateAutomation": {
            "notifyProjectOwnerEnabled": true,
            "notifyProjectOwnerEmailTemplate": {
              "uid": "template-target",
              "name": "Target template"
            }
          }
        }
        """;

        var result = JObject.Parse(AutomatedProjectSettingsPayloadBuilder.Build(
            existingSettingsJson,
            new UpdateAutomatedProjectSettingsRequest()));

        Assert.AreEqual("template-create",
            result["createProjectAutomation"]!["notifyProjectOwnerEmailTemplateUid"]!.Value<string>());
        Assert.IsNull(result["createProjectAutomation"]!["notifyProjectOwnerEmailTemplate"]);

        Assert.AreEqual("template-source",
            result["sourceUpdateAutomation"]!["notifyProjectOwnerEmailTemplateUid"]!.Value<string>());
        Assert.IsNull(result["sourceUpdateAutomation"]!["notifyProjectOwnerEmailTemplate"]);

        Assert.AreEqual("template-target",
            result["targetUpdateAutomation"]!["notifyProjectOwnerEmailTemplateUid"]!.Value<string>());
        Assert.IsNull(result["targetUpdateAutomation"]!["notifyProjectOwnerEmailTemplate"]);
    }
}
