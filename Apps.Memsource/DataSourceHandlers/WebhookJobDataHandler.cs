using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class WebhookJobDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] ProjectOptionalRequest projectRequest)
    : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private ProjectOptionalRequest ProjectRequest { get; } = projectRequest;

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ProjectRequest.ProjectUId))
        {
            return [];
        }

        var projectRequest = new RestRequest($"/api2/v1/projects/{ProjectRequest.ProjectUId}", Method.Get);
        var project = await Client.ExecuteWithHandling<ProjectDto>(projectRequest);

        var workflowLevels = project.WorkflowSteps?
            .Select(x => x.WorkflowLevel)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        if (workflowLevels == null || workflowLevels.Count == 0)
        {
            workflowLevels = [1];
        }

        var jobs = new List<ListJobDto>();
        foreach (var workflowLevel in workflowLevels)
        {
            var request = new RestRequest($"/api2/v2/projects/{ProjectRequest.ProjectUId}/jobs", Method.Get);
            request.AddQueryParameter("workflowLevel", workflowLevel);

            if (!string.IsNullOrWhiteSpace(context.SearchString))
            {
                request.AddQueryParameter("filename", context.SearchString);
            }

            var levelJobs = await Client.PaginateOnce<ListJobDto>(request);
            jobs.AddRange(levelJobs);
        }

        return jobs
            .GroupBy(x => x.Uid)
            .Select(g => g.First())
            .OrderBy(x => x.Filename)
            .ThenBy(x => x.WorkflowStep?.WorkflowLevel)
            .Select(x => new DataSourceItem(x.Uid, BuildDisplayName(x)));
    }

    private static string BuildDisplayName(ListJobDto job)
    {
        var workflowLabel = !string.IsNullOrWhiteSpace(job.WorkflowStep?.Name)
            ? job.WorkflowStep.Name
            : $"Level {job.WorkflowStep?.WorkflowLevel ?? 0}";

        return $"{job.Filename} ({job.InnerId}) [{workflowLabel}]";
    }
}
