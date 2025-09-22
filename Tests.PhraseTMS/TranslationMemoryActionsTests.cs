using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.TranslationMemories.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class TranslationMemoryActionsTests : TestBase
{
    private TranslationMemoryActions TranslationMemoryActions => new(InvocationContext, FileManager);

    private const string TranslationMemoryId = "Hv9w0ZZz9DPimGmJJSU7g0";

    [TestMethod]
    public async Task UpdateTmInsertSegmentsFromFile_BasicXliffFile_Success()
    {
        var updateTmRequest = new UpdateTmRequest
        {
            TranslationMemoryUId = TranslationMemoryId,
            File = new FileReference { Name = "basic22.xliff" },
            TargetLanguage = "fr"
        };

        await TranslationMemoryActions.UpdateTmInsertSegmentsFromFile(updateTmRequest);
    }
    
    [TestMethod]
    public async Task UpdateTmInsertSegmentsFromFile_ComplexXliffFile_Success()
    {
        var updateTmRequest = new UpdateTmRequest
        {
            TranslationMemoryUId = TranslationMemoryId,
            File = new FileReference { Name = "complex22.xliff" },
            TargetLanguage = "fr"
        };

        await TranslationMemoryActions.UpdateTmInsertSegmentsFromFile(updateTmRequest);
    }
}