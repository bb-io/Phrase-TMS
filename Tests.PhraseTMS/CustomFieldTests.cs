using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.CustomFields;
using Apps.PhraseTMS.Models.Projects.Requests;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class CustomFieldTests : TestBase
    {
        [TestMethod]
        public async Task GetCustomUrlField_IsSuccess()
        {
            var action = new CustomFieldsActions(InvocationContext);

            var response = await action.GetUrlCustomField(new ProjectRequest { ProjectUId= "B1hg3UPb3dQoqaIheND4D5" },new UrlCustomFieldRequest { FieldUId= "qUTQ7vF7YpoP9Rhewu2lg0" });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task SetUrlCustomField_IsSuccess()
        {
            var action = new CustomFieldsActions(InvocationContext);

            await action.SetUrlCustomField(new ProjectRequest { ProjectUId = "B1hg3UPb3dQoqaIheND4D5" }, new UrlCustomFieldRequest { FieldUId = "qUTQ7vF7YpoP9Rhewu2lg0" },
                "https://www.larksuite.com/hc/en-US/articles");

            Assert.IsTrue(true);
        }
    }
}
