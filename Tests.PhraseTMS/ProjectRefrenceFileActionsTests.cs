using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class ProjectRefrenceFileActionsTests : TestBaseMultipleConnections
{
    [TestMethod, ContextDataSource(ConnectionTypes.ApiToken)]
    public async Task DownloadReferenceFiles_IsSuccess(InvocationContext context)
    {
        // Arrange
        var actions = new ProjectRefrenceFileActions(context, FileManager);
        var input = new ReferenceFileRequest
        {
            ProjectUId = "FnDIVcjSkBX28pvrl2dVU0",
            ReferenceFileUId = "J0bNSjkhQrPGdqGVCqzw74"
        };

        // Act
        var result = await actions.DownloadReferenceFiles(input);

        // Assert
        TestContext.WriteLine(result.File.Name);
        Assert.IsNotNull(result.File);
    }
}