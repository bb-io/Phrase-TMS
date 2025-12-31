using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.TranslationMemories.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class TranslationMemoryActionsTests : TestBaseMultipleConnections
{
    private const string TranslationMemoryId = "Hv9w0ZZz9DPimGmJJSU7g0";

    [TestMethod, ContextDataSource]
    public async Task UpdateTmInsertSegmentsFromFile_BasicXliffFile_Success(InvocationContext context)
    {
        var actions = new TranslationMemoryActions(context, FileManager);
        var updateTmRequest = new UpdateTmRequest
        {
            TranslationMemoryUId = TranslationMemoryId,
            File = new FileReference { Name = "basic22.xliff" },
            TargetLanguage = "fr"
        };

        await actions.UpdateTmInsertSegmentsFromFile(updateTmRequest);
    }

    [TestMethod, ContextDataSource]
    public async Task UpdateTmInsertSegmentsFromFile_ComplexXliffFile_Success(InvocationContext context)
    {
        var actions = new TranslationMemoryActions(context, FileManager);
        var updateTmRequest = new UpdateTmRequest
        {
            TranslationMemoryUId = TranslationMemoryId,
            File = new FileReference { Name = "complex22.xliff" },
            TargetLanguage = "fr"
        };

        await actions.UpdateTmInsertSegmentsFromFile(updateTmRequest);
    }

    [TestMethod, ContextDataSource]
    public async Task UpdateTmInsertSegmentsFromFile_FilterSegments_Success(InvocationContext context)
    {
        var actions = new TranslationMemoryActions(context, FileManager);
        var updateTmRequest = new UpdateTmRequest
        {
            TranslationMemoryUId = TranslationMemoryId,
            File = new FileReference { Name = "basic-interoperable.xliff" },
            SegmentStates = ["initial"],
        };

        await actions.UpdateTmInsertSegmentsFromFile(updateTmRequest);
    }
}