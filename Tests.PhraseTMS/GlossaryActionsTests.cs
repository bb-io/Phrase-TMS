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
}
