using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Users.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class UserTests : TestBaseMultipleConnections
{
    [TestMethod]
    public async Task SearchUsers_IsSuccess(InvocationContext context)
    {
        var actions = new UserActions(context);
        var result = await actions.ListAllUsers(new ListAllUsersQuery { role = ["ADMIN"], includeDeleted = false });
        Assert.IsNotNull(result);
        PrintResult(result);
    }

    [TestMethod]
    public async Task FindUser_IsSuccess(InvocationContext context)
    {
        var actions = new UserActions(context);
        var result = await actions.FindUser(new ListAllUsersQuery { role = ["ADMIN"], includeDeleted = false });
        Assert.IsNotNull(result);
        PrintResult(result);
    }
}
