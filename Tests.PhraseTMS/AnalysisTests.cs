using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Clients.Requests;
using Newtonsoft.Json;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class ClientTests : TestBase
    {
        public const string CLIENT_ID = "qdEcGUaxnLrs4d6z4B89c0";

        [TestMethod]
        public async Task Search_clients_works()
        {
            var actions = new ClientActions(InvocationContext);

            var result = await actions.ListClients(new ListClientsQuery { });

            Assert.IsTrue(result.Clients.Any() && result.Clients.All(x => x.UId != null));
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task Get_client_works()
        {
            var actions = new ClientActions(InvocationContext);

            var result = await actions.GetClient(new ClientRequest { ClientUid = CLIENT_ID });

            Assert.IsTrue(result.UId != null);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

    }
}
