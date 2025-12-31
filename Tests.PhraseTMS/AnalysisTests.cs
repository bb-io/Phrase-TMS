using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Clients.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class ClientTests : TestBaseMultipleConnections
{
    public const string CLIENT_ID = "qdEcGUaxnLrs4d6z4B89c0";

    [TestMethod, ContextDataSource]
    public async Task Search_clients_works(InvocationContext context)
    {
        var actions = new ClientActions(context);

        var result = await actions.ListClients(new ListClientsQuery { });

        PrintResult(result);
        Assert.IsTrue(result.Clients.Any() && result.Clients.All(x => x.UId != null));
    }

    [TestMethod, ContextDataSource]
    public async Task Get_client_works(InvocationContext context)
    {
        var actions = new ClientActions(context);

        var result = await actions.GetClient(new ClientRequest { ClientUid = CLIENT_ID });

        PrintResult(result);
        Assert.IsNotNull(result);
    }
}
