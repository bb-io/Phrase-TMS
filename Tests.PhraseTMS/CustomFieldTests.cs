using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.CustomFields;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class CustomFieldTests : TestBaseMultipleConnections
{
    [TestMethod, ContextDataSource]
    public async Task SetDateCustomField_works(InvocationContext context)
    {
        // Arrange
        var actions = new CustomFieldsActions(context);
        var date = DateTime.UtcNow.AddDays(15);
        var project = new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" };
        var dateCustomField = new DateCustomFieldRequest { FieldUId = "gtCnCd6aZ0SkaGXu8wETa1" };

        // Act
        await actions.SetDateCustomField(project, dateCustomField, date);
    }
    
    [TestMethod, ContextDataSource]
    public async Task GetCustomUrlField_IsSuccess(InvocationContext context)
    {
        // Arrange
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = "B1hg3UPb3dQoqaIheND4D5" };
        var customField = new UrlCustomFieldRequest { FieldUId = "qUTQ7vF7YpoP9Rhewu2lg0" };

        // Act
        string? response = await action.GetUrlCustomField(project, customField);

        // Assert
        TestContext.WriteLine(response);
        Assert.IsNotNull(response);
    }

    [TestMethod, ContextDataSource]
    public async Task SetUrlCustomField_IsSuccess(InvocationContext context)
    {
        // Arrange
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = "xc5XRBM51xQG9aaFBzUKp6" };
        var customField = new UrlCustomFieldRequest { FieldUId = "W8RnxMfxGtoz9t6P34zxE0" };

        // Act
        await action.SetUrlCustomField(project, customField, "https://www.larksuite.com/hc/en-US/articles/test");
    }
}
