using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Conversation;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class ConversationTests : TestBase
    {
        [TestMethod]
        public async Task Get_conversation_works()
        {
            var actions = new ConversationActions(InvocationContext);
            var conv = new GetConversationRequest
            {
                JobUId = "ftRN9yMaryr4fRUYYbdX42",
                ConversationUId= "8eb9ddb1_d052_4b76_b6c3_32d600a8e919"
            };
            var result = await actions.GetConversation(new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" }, conv);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task Search_conversations_works()
        {
            var actions = new ConversationActions(InvocationContext);
            var conv = new JobRequest
            {
                JobUId = "ftRN9yMaryr4fRUYYbdX42"              
            };
            var search = new SearchConversationRequest
            {
                //IncludeDeleted = false,
                Since = DateTime.UtcNow.AddDays(-5)
            };
            var result = await actions.SearchConversations(new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" }, conv, search);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Delete_conversation_works()
        {
            var actions = new ConversationActions(InvocationContext);
            var conv = new GetConversationRequest
            {
                JobUId = "ftRN9yMaryr4fRUYYbdX42",
                ConversationUId = "8eb9ddb1_d052_4b76_b6c3_32d600a8e919"
            };
            await actions.DeleteConversation(new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" }, conv);
            Assert.IsTrue(true);
        }
    }
}
