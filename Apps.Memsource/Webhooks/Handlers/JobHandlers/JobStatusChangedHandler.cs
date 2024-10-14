using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class JobStatusChangedHandler(
    InvocationContext invocationContext,
    [WebhookParameter] JobStatusChangedRequest statusRequest,
    [WebhookParameter] ProjectOptionalRequest projectOptionalRequest,
    [WebhookParameter] OptionalJobRequest jobOptionalRequest)
    : BaseWebhookHandler(SubscriptionEvent), IAfterSubscriptionWebhookEventHandler<JobResponse>
{
    const string SubscriptionEvent = "JOB_STATUS_CHANGED";
    
    public async Task<AfterSubscriptionEventResponse<JobResponse>> OnWebhookSubscribedAsync()
    {
        if (jobOptionalRequest.JobUId != null && statusRequest.Status != null && projectOptionalRequest.ProjectUId != null)
        {
            var client = new PhraseTmsClient(invocationContext.AuthenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{projectOptionalRequest.ProjectUId}/jobs/{jobOptionalRequest.JobUId}",
                Method.Get, invocationContext.AuthenticationCredentialsProviders);

            var response = await client.ExecuteWithHandling<JobDto>(request);

            var result = new JobResponse
            {
                Uid = response.Uid,
                Filename = response.Filename,
                TargetLanguage = response.TargetLang,
                Status = response.Status,
                ProjectUid = response.Project.UId,
                WordCount = response.WordsCount,
                SourceLanguage = response.SourceLang,
            };
            
            if(result.Status == statusRequest.Status)
            {
                return new AfterSubscriptionEventResponse<JobResponse>
                {
                    Result = result
                };
            }
        }

        return null;
    }
}