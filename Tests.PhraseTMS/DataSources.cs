using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Conversations.Requests;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class DataSources : TestBaseMultipleConnections
{
    public const string EMPTY_PROJECT_ID = "YWxQLsQXtwbN2FnxwoSFx0";

    private async Task Test(IAsyncDataSourceItemHandler handler, string search = "")
    {
        // Act
        var response = await handler.GetDataAsync(new DataSourceContext { SearchString = search }, CancellationToken.None);

        TestContext.WriteLine($"Total: {response.Count()}");
        PrintDataHandlerResult(response);

        // Assert
        Assert.IsNotNull(response, "Response should not be null");

        if (!string.IsNullOrEmpty(search))
            Assert.IsTrue(response.Any(), "Response should return at least 1 value");
    }

    [TestMethod, ContextDataSource]
    public async Task Business_units_returns_values(InvocationContext context) 
        => await Test(new BusinessUnitDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Business_units_with_query_returns_values(InvocationContext context) 
        => await Test(new BusinessUnitDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task Client_returns_values(InvocationContext context) 
        => await Test(new ClientDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Client_with_query_returns_values(InvocationContext context) 
        => await Test(new ClientDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task Domain_returns_values(InvocationContext context) 
        => await Test(new DomainDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Domain_with_query_returns_values(InvocationContext context) 
        => await Test(new DomainDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task Job_query_returns_values(InvocationContext context) 
        => await Test(new JobDataHandler(context, new ProjectRequest { ProjectUId = "OHocQVUqGBFacBtS7HYhq2" }));

    [TestMethod, ContextDataSource]
    public async Task Language_returns_values(InvocationContext context) 
        => await Test(new LanguageDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Language_with_query_returns_values(InvocationContext context) 
        => await Test(new LanguageDataHandler(context), "english");

    [TestMethod, ContextDataSource]
    public async Task Lqa_returns_values(InvocationContext context) 
        => await Test(new LqaProfileDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Lqa_with_query_returns_values(InvocationContext context) 
        => await Test(new LqaProfileDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task Netrate_returns_values(InvocationContext context) 
        => await Test(new NetRateSchemeDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Netrate_with_query_returns_values(InvocationContext context) 
        => await Test(new NetRateSchemeDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task Pricelist_returns_values(InvocationContext context) 
        => await Test(new PriceListDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Project_returns_values(InvocationContext context) 
        => await Test(new ProjectDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Project_with_query_returns_values(InvocationContext context) 
        => await Test(new ProjectDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task Project_template_returns_values(InvocationContext context) 
        => await Test(new ProjectTemplateDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Project_template_with_query_returns_values(InvocationContext context) 
        => await Test(new ProjectTemplateDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task CustomFields_returns_values(InvocationContext context) 
        => await Test(new CustomFieldTextDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Reference_file_returns_values(InvocationContext context) 
        => await Test(new ReferenceFileDataHandler(context, new ReferenceFileRequest { ProjectUId = EMPTY_PROJECT_ID }));

    [TestMethod, ContextDataSource]
    public async Task Subdomain_returns_values(InvocationContext context) 
        => await Test(new SubdomainDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Subdomain_with_query_returns_values(InvocationContext context) 
        => await Test(new SubdomainDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task Termbase_returns_values(InvocationContext context) 
        => await Test(new TermBaseDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Termbase_with_query_returns_values(InvocationContext context) 
        => await Test(new TermBaseDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task Tm_returns_values(InvocationContext context) 
        => await Test(new TmDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Tm_with_query_returns_values(InvocationContext context) 
        => await Test(new TmDataHandler(context), "test");

    [TestMethod, ContextDataSource]
    public async Task User_returns_values(InvocationContext context) 
        => await Test(new UserDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task User_with_query_returns_values(InvocationContext context) 
        => await Test(new UserDataHandler(context), "");

    [TestMethod, ContextDataSource]
    public async Task Vendor_returns_values(InvocationContext context) 
        => await Test(new VendorDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Workflow_returns_values(InvocationContext context) 
        => await Test(new WorkflowStepDataHandler(context));

    [TestMethod, ContextDataSource]
    public async Task Workflow_with_query_returns_values(InvocationContext context) 
        => await Test(new WorkflowStepDataHandler(context), "Translation");

    [TestMethod, ContextDataSource]
    public async Task ConversationDataHandler_ReturnsConversations(InvocationContext context)
        => await Test(new ConversationDataHandler(context, new JobRequest { JobUId = "S1Lng7SgldQMeiwPm2srx3" }));

    [TestMethod, ContextDataSource]
    public async Task CommentDataHandler_ReturnsComments(InvocationContext context)
        => await Test(new CommentDataHandler(
            context,
            new JobRequest { JobUId = "S1Lng7SgldQMeiwPm2srx3" }, 
            new ConversationRequest { ConversationUId = "9ae3cc29_cfd8_47d9_b17e_c3a71781e1b8" }
        ));

    [TestMethod, ContextDataSource]
    public async Task SegmentDataHanlder_ReturnsSegments(InvocationContext context)
        => await Test(new SegmentDataHandler(
            context,
            new JobRequest { JobUId = "dtdm5mE4e3pu25KiCbTiL3" },
            new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" }
        ));

    [TestMethod, ContextDataSource]
    public async Task CustomFieldUrlDataHandler_ReturnsFields(InvocationContext context)
        => await Test(new CustomFieldUrlDataHandler(context));
}
