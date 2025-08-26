using Apps.PhraseTMS.Models.Users.Requests;
using Apps.PhraseTMS.Polling;
using Apps.PhraseTMS.Polling.Models;
using Blackbird.Applications.Sdk.Common.Polling;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class PollingTests : TestBase
    {
        [TestMethod]
        public async Task OnUsersCreated_IsSuccess()
        {
            var polling = new UserPollingList(InvocationContext);
            var input = new ListAllUsersQuery
            {
                role = ["ADMIN"],
                includeDeleted = false
            };

            var result = await polling.OnUsersCreated(new PollingEventRequest<PollingMemory>
            {
                Memory = new PollingMemory
                {
                    LastPollingTime = DateTime.UtcNow.AddMonths(-10)
                },
            }, input);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }
    }
}
