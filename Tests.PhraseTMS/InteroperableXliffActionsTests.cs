
using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Filters.Transformations;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class InteroperableXliffActionsTests : TestBaseMultipleConnections
{
    public const string PROJECT_ID = "0dmkbDfIkgTMrOgCf3l5c0";
    public const string JOB_ID = "a4n2zcUF4gaFb5MpTTXTl2";

    [TestMethod, ContextDataSource]
    public async Task DownloadInteroperableXliff_IsSuccess(InvocationContext context)
    {
        // Arrange
        var actions = new InteroperableXliffActions(context, FileManager);
        var project = new ProjectRequest { ProjectUId = PROJECT_ID };
        var job = new JobRequest { JobUId = JOB_ID };

        // Act
        var result = await actions.DownloadInteroperableXliff(project, job);

        // Assert
        Console.WriteLine(result.File.Name);
        Assert.AreEqual(6, result.TotalSegments);
    }
}
