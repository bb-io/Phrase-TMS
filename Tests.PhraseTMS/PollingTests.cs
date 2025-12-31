using Apps.PhraseTMS.Models.Users.Requests;
using Apps.PhraseTMS.Polling;
using Apps.PhraseTMS.Polling.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class PollingTests : TestBaseMultipleConnections
{
    [TestMethod, ContextDataSource]
    public async Task OnUsersCreated_IsSuccess(InvocationContext context)
    {
        // Arrange
        var polling = new UserPollingList(context);
        var input = new ListAllUsersQuery
        {
            role = ["ADMIN"],
            includeDeleted = false
        };
        var memory = new PollingEventRequest<PollingMemory>
        {
            Memory = new PollingMemory
            {
                LastPollingTime = DateTime.UtcNow.AddMonths(-10)
            },
        };

        // Act
        var result = await polling.OnUsersCreated(memory, input);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }
}
