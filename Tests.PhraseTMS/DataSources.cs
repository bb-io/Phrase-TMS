using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Dynamic;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class DataSources : TestBase
    {
        public const string EMPTY_PROJECT_ID = "dmXznxlJx790symN3ig5d7";

        private async Task Test(IAsyncDataSourceItemHandler handler, string search = "")
        {
            // Act
            var response = await handler.GetDataAsync(new DataSourceContext { SearchString = search }, CancellationToken.None);

            Console.WriteLine($"Total: {response.Count()}");
            foreach (var item in response)
            {
                Console.WriteLine($"{item.Value}: {item.DisplayName}");
            }

            // Assert
            Assert.IsNotNull(response, "Response should not be null");

            if (!string.IsNullOrEmpty(search))
                Assert.IsTrue(response.Any(), "Response should return at least 1 value");
        }

        [TestMethod]
        public async Task Business_units_returns_values() => await Test(new BusinessUnitDataHandler(InvocationContext));

        [TestMethod]
        public async Task Business_units_with_query_returns_values() => await Test(new BusinessUnitDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task Client_returns_values() => await Test(new ClientDataHandler(InvocationContext));

        [TestMethod]
        public async Task Client_with_query_returns_values() => await Test(new ClientDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task Domain_returns_values() => await Test(new DomainDataHandler(InvocationContext));

        [TestMethod]
        public async Task Domain_with_query_returns_values() => await Test(new DomainDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task Job_query_returns_values() => await Test(new JobDataHandler(InvocationContext, new ProjectRequest { ProjectUId = EMPTY_PROJECT_ID }));

        [TestMethod]
        public async Task Language_returns_values() => await Test(new LanguageDataHandler(InvocationContext));

        [TestMethod]
        public async Task Language_with_query_returns_values() => await Test(new LanguageDataHandler(InvocationContext), "english");

        [TestMethod]
        public async Task Lqa_returns_values() => await Test(new LqaProfileDataHandler(InvocationContext));

        [TestMethod]
        public async Task Lqa_with_query_returns_values() => await Test(new LqaProfileDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task Netrate_returns_values() => await Test(new NetRateSchemeDataHandler(InvocationContext));

        [TestMethod]
        public async Task Netrate_with_query_returns_values() => await Test(new NetRateSchemeDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task Pricelist_returns_values() => await Test(new PriceListDataHandler(InvocationContext));

        [TestMethod]
        public async Task Project_returns_values() => await Test(new ProjectDataHandler(InvocationContext));

        [TestMethod]
        public async Task Project_with_query_returns_values() => await Test(new ProjectDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task Project_template_returns_values() => await Test(new ProjectTemplateDataHandler(InvocationContext));

        [TestMethod]
        public async Task Project_template_with_query_returns_values() => await Test(new ProjectTemplateDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task Reference_file_returns_values() => await Test(new ReferenceFileDataHandler(InvocationContext, new ReferenceFileRequest { ProjectUId = EMPTY_PROJECT_ID }));

        [TestMethod]
        public async Task Subdomain_returns_values() => await Test(new SubdomainDataHandler(InvocationContext));

        [TestMethod]
        public async Task Subdomain_with_query_returns_values() => await Test(new SubdomainDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task Termbase_returns_values() => await Test(new TermBaseDataHandler(InvocationContext));

        [TestMethod]
        public async Task Termbase_with_query_returns_values() => await Test(new TermBaseDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task Tm_returns_values() => await Test(new TmDataHandler(InvocationContext));

        [TestMethod]
        public async Task Tm_with_query_returns_values() => await Test(new TmDataHandler(InvocationContext), "test");

        [TestMethod]
        public async Task User_returns_values() => await Test(new UserDataHandler(InvocationContext));

        [TestMethod]
        public async Task User_with_query_returns_values() => await Test(new UserDataHandler(InvocationContext), "Vitalii");

        [TestMethod]
        public async Task Vendor_returns_values() => await Test(new VendorDataHandler(InvocationContext));

        [TestMethod]
        public async Task Workflow_returns_values() => await Test(new WorkflowStepDataHandler(InvocationContext));

        [TestMethod]
        public async Task Workflow_with_query_returns_values() => await Test(new WorkflowStepDataHandler(InvocationContext), "Translation");
    }
}
