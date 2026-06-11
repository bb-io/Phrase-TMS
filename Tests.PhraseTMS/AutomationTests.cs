using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.AutomatedSettings.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class AutomationTests : TestBaseMultipleConnections 
{
    [TestMethod, ContextDataSource]
    public async Task UpdateAutomatedProjectSettings_ReturnsUpdatedSetting(InvocationContext invocationContext)
    {
        // Arrange
        var actions = new AutomationActions(invocationContext);
        var settingsRequest = new AutomatedProjectSettingRequest { SettingId = "ZRNAcLs7aaoJ6Bfb0J4rz0" };
        string newTargetLang = "de";
        var input = new UpdateAutomatedProjectSettingsRequest
        {
            SelectedTargetLanguages = [newTargetLang],
        };

        // Act
        var result = await actions.UpdateAutomatedProjectSettings(settingsRequest, input);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.SelectedTargetLanguages.Contains(newTargetLang));
    }
}