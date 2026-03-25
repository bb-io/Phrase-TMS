using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.QualityAssurance.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class LqaErrorCategoryDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] JobRequest job,
    [ActionParameter] ProjectRequest project
) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var lqaSettings = await GetLqaSettings();
        var categories = FlattenCategories(lqaSettings.Categories)
            .Where(x => context.SearchString == null || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase));

        return categories.Select(x => new DataSourceItem(x.ErrorCategoryId.ToString(), x.Name));
    }

    private async Task<LqaSettingsResponse> GetLqaSettings()
    {
        var jobRequest = new RestRequest($"/api2/v1/projects/{project.ProjectUId}/jobs/{job.JobUId}", Method.Get);
        var jobResponse = await Client.ExecuteWithHandling<JobDto>(jobRequest);

        var lqaRequest = new RestRequest($"/api2/v1/projects/{project.ProjectUId}/lqaSettings", Method.Get);
        lqaRequest.AddQueryParameter("workflowLevel", jobResponse.WorkflowStep.WorkflowLevel);

        return await Client.ExecuteWithHandling<LqaSettingsResponse>(lqaRequest);
    }

    private static IEnumerable<LqaCategoryDto> FlattenCategories(IEnumerable<LqaCategoryDto> categories)
    {
        foreach (var category in categories)
        {
            yield return category;

            if (category.Children == null)
                continue;

            foreach (var child in FlattenCategories(category.Children))
                yield return child;
        }
    }
}
