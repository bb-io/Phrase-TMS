using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.CustomFields;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class CustomFieldTests : TestBaseMultipleConnections
{
    private const string JOB_CUSTOM_FIELD_PROJECT_ID = "";
    private const string JOB_CUSTOM_FIELD_JOB_ID = "";
    private const string JOB_TEXT_FIELD_ID = "";
    private const string JOB_NUMBER_FIELD_ID = "";
    private const string JOB_DATE_FIELD_ID = "";
    private const string JOB_URL_FIELD_ID = "";
    private const string JOB_SINGLE_SELECT_FIELD_ID = "";
    private const string JOB_SINGLE_SELECT_OPTION_ID = "";
    private const string JOB_MULTI_SELECT_FIELD_ID = "";
    private static readonly IEnumerable<string> JOB_MULTI_SELECT_OPTION_IDS = Array.Empty<string>();

    [TestMethod, ContextDataSource]
    public async Task SetDateCustomField_works(InvocationContext context)
    {
        var actions = new CustomFieldsActions(context);
        var date = DateTime.UtcNow.AddDays(15);
        var project = new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" };
        var dateCustomField = new DateCustomFieldRequest { FieldUId = "gtCnCd6aZ0SkaGXu8wETa1" };

        await actions.SetDateCustomField(project, dateCustomField, date);
    }

    [TestMethod, ContextDataSource]
    public async Task GetCustomUrlField_IsSuccess(InvocationContext context)
    {
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = "B1hg3UPb3dQoqaIheND4D5" };
        var customField = new UrlCustomFieldRequest { FieldUId = "qUTQ7vF7YpoP9Rhewu2lg0" };

        var response = await action.GetUrlCustomField(project, customField);

        TestContext.WriteLine(response);
        Assert.IsNotNull(response);
    }

    [TestMethod, ContextDataSource]
    public async Task SetUrlCustomField_IsSuccess(InvocationContext context)
    {
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = "xc5XRBM51xQG9aaFBzUKp6" };
        var customField = new UrlCustomFieldRequest { FieldUId = "W8RnxMfxGtoz9t6P34zxE0" };

        await action.SetUrlCustomField(project, customField, "https://www.larksuite.com/hc/en-US/articles/test");
    }

    [TestMethod, ContextDataSource]
    public async Task SetAndGetJobTextCustomField_IsSuccess(InvocationContext context)
    {
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = GetRequiredValue(JOB_CUSTOM_FIELD_PROJECT_ID, nameof(JOB_CUSTOM_FIELD_PROJECT_ID)) };
        var job = new JobRequest { JobUId = GetRequiredValue(JOB_CUSTOM_FIELD_JOB_ID, nameof(JOB_CUSTOM_FIELD_JOB_ID)) };
        var customField = new JobTextCustomFieldRequest { FieldUId = GetRequiredValue(JOB_TEXT_FIELD_ID, nameof(JOB_TEXT_FIELD_ID)) };
        var expectedValue = $"bb-job-text-{DateTime.UtcNow:yyyyMMddHHmmss}";

        await action.SetJobTextCustomField(project, job, customField, expectedValue);
        var response = await action.GetJobTextCustomField(project, job, customField);

        Assert.AreEqual(expectedValue, response);
    }

    [TestMethod, ContextDataSource]
    public async Task SetAndGetJobNumberCustomField_IsSuccess(InvocationContext context)
    {
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = GetRequiredValue(JOB_CUSTOM_FIELD_PROJECT_ID, nameof(JOB_CUSTOM_FIELD_PROJECT_ID)) };
        var job = new JobRequest { JobUId = GetRequiredValue(JOB_CUSTOM_FIELD_JOB_ID, nameof(JOB_CUSTOM_FIELD_JOB_ID)) };
        var customField = new JobNumberCustomFieldRequest { FieldUId = GetRequiredValue(JOB_NUMBER_FIELD_ID, nameof(JOB_NUMBER_FIELD_ID)) };
        const double expectedValue = 42.5d;

        await action.SetJobNumberCustomField(project, job, customField, expectedValue);
        var response = await action.GetJobNumberCustomField(project, job, customField);

        Assert.AreEqual(expectedValue, response);
    }

    [TestMethod, ContextDataSource]
    public async Task SetAndGetJobDateCustomField_IsSuccess(InvocationContext context)
    {
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = GetRequiredValue(JOB_CUSTOM_FIELD_PROJECT_ID, nameof(JOB_CUSTOM_FIELD_PROJECT_ID)) };
        var job = new JobRequest { JobUId = GetRequiredValue(JOB_CUSTOM_FIELD_JOB_ID, nameof(JOB_CUSTOM_FIELD_JOB_ID)) };
        var customField = new JobDateCustomFieldRequest { FieldUId = GetRequiredValue(JOB_DATE_FIELD_ID, nameof(JOB_DATE_FIELD_ID)) };
        var expectedValue = DateTime.UtcNow.Date.AddDays(14);

        await action.SetJobDateCustomField(project, job, customField, expectedValue);
        var response = await action.GetJobDateCustomField(project, job, customField);

        Assert.AreEqual(expectedValue.Date, response?.Date);
    }

    [TestMethod, ContextDataSource]
    public async Task SetAndGetJobUrlCustomField_IsSuccess(InvocationContext context)
    {
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = GetRequiredValue(JOB_CUSTOM_FIELD_PROJECT_ID, nameof(JOB_CUSTOM_FIELD_PROJECT_ID)) };
        var job = new JobRequest { JobUId = GetRequiredValue(JOB_CUSTOM_FIELD_JOB_ID, nameof(JOB_CUSTOM_FIELD_JOB_ID)) };
        var customField = new JobUrlCustomFieldRequest { FieldUId = GetRequiredValue(JOB_URL_FIELD_ID, nameof(JOB_URL_FIELD_ID)) };
        var expectedValue = $"https://example.com/job-custom-field/{DateTime.UtcNow:yyyyMMddHHmmss}";

        await action.SetJobUrlCustomField(project, job, customField, expectedValue);
        var response = await action.GetJobUrlCustomField(project, job, customField);

        Assert.AreEqual(expectedValue, response);
    }

    [TestMethod, ContextDataSource]
    public async Task SetAndGetJobSingleSelectCustomField_IsSuccess(InvocationContext context)
    {
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = GetRequiredValue(JOB_CUSTOM_FIELD_PROJECT_ID, nameof(JOB_CUSTOM_FIELD_PROJECT_ID)) };
        var job = new JobRequest { JobUId = GetRequiredValue(JOB_CUSTOM_FIELD_JOB_ID, nameof(JOB_CUSTOM_FIELD_JOB_ID)) };
        var customField = new JobSingleSelectCustomFieldRequest { FieldUId = GetRequiredValue(JOB_SINGLE_SELECT_FIELD_ID, nameof(JOB_SINGLE_SELECT_FIELD_ID)) };
        var selectedOption = new JobSelectedOptionRequest { OptionUId = GetRequiredValue(JOB_SINGLE_SELECT_OPTION_ID, nameof(JOB_SINGLE_SELECT_OPTION_ID)) };

        await action.SetJobSingleSelectCustomField(project, job, customField, selectedOption);
        var response = await action.GetJobSingleSelectCustomField(project, job, customField);

        Assert.IsNotNull(response);
    }

    [TestMethod, ContextDataSource]
    public async Task GetJobMultiSelectCustomField_IsSuccess(InvocationContext context)
    {
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = GetRequiredValue(JOB_CUSTOM_FIELD_PROJECT_ID, nameof(JOB_CUSTOM_FIELD_PROJECT_ID)) };
        var job = new JobRequest { JobUId = GetRequiredValue(JOB_CUSTOM_FIELD_JOB_ID, nameof(JOB_CUSTOM_FIELD_JOB_ID)) };
        var customField = new JobMultiSelectCustomFieldRequest { FieldUId = GetRequiredValue(JOB_MULTI_SELECT_FIELD_ID, nameof(JOB_MULTI_SELECT_FIELD_ID)) };

        var response = await action.GetJobMultiSelectCustomField(project, job, customField);

        Assert.IsNotNull(response);
    }

    [TestMethod, ContextDataSource]
    public async Task SetAndGetJobMultiSelectCustomField_IsSuccess(InvocationContext context)
    {
        var action = new CustomFieldsActions(context);
        var project = new ProjectRequest { ProjectUId = GetRequiredValue(JOB_CUSTOM_FIELD_PROJECT_ID, nameof(JOB_CUSTOM_FIELD_PROJECT_ID)) };
        var job = new JobRequest { JobUId = GetRequiredValue(JOB_CUSTOM_FIELD_JOB_ID, nameof(JOB_CUSTOM_FIELD_JOB_ID)) };
        var customField = new JobMultiSelectCustomFieldRequest { FieldUId = GetRequiredValue(JOB_MULTI_SELECT_FIELD_ID, nameof(JOB_MULTI_SELECT_FIELD_ID)) };
        var selectedOptions = new JobSelectedOptionsRequest
        {
            OptionUIds = GetRequiredValues(JOB_MULTI_SELECT_OPTION_IDS, nameof(JOB_MULTI_SELECT_OPTION_IDS))
        };

        await action.SetJobMultiSelectCustomField(project, job, customField, selectedOptions);
        var response = await action.GetJobMultiSelectCustomField(project, job, customField);

        Assert.IsTrue(response.Results.Any());
    }

    private static string GetRequiredValue(string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            Assert.Inconclusive($"Fill in {name} in {nameof(CustomFieldTests)} before running this test");

        return value;
    }

    private static IEnumerable<string> GetRequiredValues(IEnumerable<string> values, string name)
    {
        var result = values.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        if (!result.Any())
            Assert.Inconclusive($"Fill in {name} in {nameof(CustomFieldTests)} before running this test");

        return result;
    }
}
