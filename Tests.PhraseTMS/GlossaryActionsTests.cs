using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Glossary.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class GlossaryActionsTests : TestBaseMultipleConnections
{
    [TestMethod, ContextDataSource]
    public async Task ExportGlossary_CorrectRequest_ReturnsGlossary(InvocationContext context)
    {
		// Arrange
		var action = new GlossaryActions(context, FileManager);
		var request = new ExportGlossaryRequest { GlossaryUId = "EaZpWNsRTmbP9NEDxHlMl1" };

		// Act
		var result = await action.ExportGlossary(request);

		// Assert
		PrintResult(result);
		Assert.IsNotNull(result);
	}

    [TestMethod, ContextDataSource]
    public async Task ImportGlossary_CorrectRequest_ReturnsGlossary(InvocationContext context)
    {
        // Arrange
        var action = new GlossaryActions(context, FileManager);
        var request = new ImportGlossaryRequest { 
            GlossaryUId = "9N18Vm34tGRFb2Yia2EKR5", 
            File= new Blackbird.Applications.Sdk.Common.Files.FileReference { Name= "sample_v3_phrase_dca.tbx" } ,
            UpdateExistingTerms= true
        };

        // Act
        await action.ImportGlossary(request);
    }

    [TestMethod, ContextDataSource]
    public async Task ClearGlossary_CorrectRequest_IsSuccess(InvocationContext context)
    {
        // Arrange
        var actions = new GlossaryActions(context, FileManager);
        var input = new ClearGlossaryRequest { GlossaryUId = "uTHNqoRE5BVA1qLUvBC9H1" };

        // Act
        await actions.ClearGlossary(input);
    }
}
