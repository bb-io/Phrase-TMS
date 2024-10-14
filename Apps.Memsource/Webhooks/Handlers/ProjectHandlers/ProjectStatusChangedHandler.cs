﻿using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;

public class ProjectStatusChangedHandler(
    [WebhookParameter] ProjectStatusChangedRequest projectStatusChangedRequest,
    [WebhookParameter] ProjectOptionalRequest projectOptionalRequest,
    InvocationContext invocationContext)
    : BaseWebhookHandler(SubscriptionEvent), IAfterSubscriptionWebhookEventHandler<ProjectDto>
{
    const string SubscriptionEvent = "PROJECT_STATUS_CHANGED";

    public async Task<AfterSubscriptionEventResponse<ProjectDto>> OnWebhookSubscribedAsync()
    {
        if (projectOptionalRequest.ProjectUId != null && projectStatusChangedRequest.Status != null)
        {
            var client = new PhraseTmsClient(invocationContext.AuthenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{projectOptionalRequest.ProjectUId}", Method.Get,
                invocationContext.AuthenticationCredentialsProviders);
            var project = await client.ExecuteWithHandling<ProjectDto>(request);
            
            if(project.Status == projectStatusChangedRequest.Status)
            {
                return new AfterSubscriptionEventResponse<ProjectDto>()
                {
                    Result = project
                };
            }
        }

        return null;
    }
}