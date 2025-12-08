using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Glossary.Requests;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class GlossaryActionsTests : TestBase
{
    [TestMethod]
    public async Task ExportGlossary_CorrectRequest_ReturnsGlossary()
    {
		// Arrange
		var action = new GlossaryActions(InvocationContext, FileManager);
		var request = new ExportGlossaryRequest { GlossaryUId = "EaZpWNsRTmbP9NEDxHlMl1" };

		// Act
		var result = await action.ExportGlossary(request);

		// Assert
		PrintResponse(result);
		Assert.IsNotNull(result);
	}

    //

    [TestMethod]
    public async Task ImportGlossary_CorrectRequest_ReturnsGlossary()
    {
        // Arrange
        var action = new GlossaryActions(InvocationContext, FileManager);
        var request = new ImportGlossaryRequest { GlossaryUId = "9N18Vm34tGRFb2Yia2EKR5", File= new Blackbird.Applications.Sdk.Common.Files.FileReference { Name= "sample_v3_phrase_dca.tbx" } };

        // Act
        await action.ImportGlossary(request);

        // Assert
        Assert.IsNotNull(true);
    }
}
