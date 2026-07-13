using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class PreTranslationFinishedHandler(
    InvocationContext invocationContext,
    [WebhookParameter] ProjectOptionalRequest projectOptionalRequest,
    [WebhookParameter] PreTranslationFinishedRequest request)
    : BaseWebhookHandler(invocationContext, SubscriptionEvent), IAfterSubscriptionWebhookEventHandler<JobResponse>
{
    private const string SubscriptionEvent = "PRE_TRANSLATION_FINISHED";

    public async Task<AfterSubscriptionEventResponse<JobResponse>> OnWebhookSubscribedAsync()
    {
        if (string.IsNullOrWhiteSpace(projectOptionalRequest.ProjectUId) ||
            string.IsNullOrWhiteSpace(request.JobUId))
        {
            return null!;
        }

        var client = new PhraseTmsClient(InvocationContext.AuthenticationCredentialsProviders);

        var countsRequest =
            new RestRequest($"/api2/v1/projects/{projectOptionalRequest.ProjectUId}/jobs/segmentsCount", Method.Post);
        countsRequest.AddJsonBody(new
        {
            jobs = new[]
            {
                new
                {
                    uid = request.JobUId
                }
            }
        });

        var countsResponse = await client.ExecuteWithHandling<GetSegmentsCountResponse>(countsRequest);
        var counts = countsResponse.SegmentsCountsResults?.FirstOrDefault()?.Counts;

        if (counts == null || !LooksPreTranslated(counts))
        {
            return null!;
        }

        var jobRequest =
            new RestRequest($"/api2/v1/projects/{projectOptionalRequest.ProjectUId}/jobs/{request.JobUId}",
                Method.Get);
        var job = await client.ExecuteWithHandling<JobDto>(jobRequest);

        return new AfterSubscriptionEventResponse<JobResponse>
        {
            Result = MapJobResponse(job)
        };
    }

    private static bool LooksPreTranslated(CountsDto counts)
        => counts.TranslatedSegmentsCount > 0
           || counts.CompletedSegmentsCount > 0
           || counts.LockedSegmentsCount > 0
           || counts.AllConfirmed;

    private static JobResponse MapJobResponse(JobDto job)
        => new()
        {
            Uid = job.Uid,
            ProjectUid = job.Project?.UId ?? string.Empty,
            ProjectName = job.Project?.Name ?? string.Empty,
            Filename = job.Filename,
            Status = job.Status,
            TargetLanguage = job.TargetLang,
            SourceLanguage = job.SourceLang,
            WordCount = job.WordsCount
        };
}
