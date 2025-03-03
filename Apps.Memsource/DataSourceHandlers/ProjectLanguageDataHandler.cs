using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Languages.Response;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;
public class ProjectLanguageDataHandler(InvocationContext invocationContext, [ActionParameter] ProjectRequest projectRequest)
    : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private ProjectRequest ProjectRequest { get; set; } = projectRequest;

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(ProjectRequest.ProjectUId))
        {
            throw new PluginMisconfigurationException("Please fill in project first");
        }
        var request = new RestRequest($"/api2/v1/projects/{ProjectRequest.ProjectUId}", Method.Get);
        var project = await Client.ExecuteWithHandling<ProjectDto>(request);
        var languages = project.TargetLangs;

        var languageRequest = new RestRequest("/api2/v1/languages", Method.Get);
        var response = await Client.ExecuteWithHandling<LanguagesResponse>(languageRequest);

        return response.Languages
            .Where(x => languages.Contains(x.Code))
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Code, x.Name));
    }
}
