using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;

public class ProjectStatusChangedHandler : BaseWebhookHandler, IAfterSubscriptionWebhookEventHandler<ProjectDto>
{
    const string SubscriptionEvent = "PROJECT_STATUS_CHANGED";
    private readonly ProjectOptionalRequest _projectOptionalRequest;
    private readonly ProjectStatusChangedRequest _projectStatusChangedRequest;
    private readonly InvocationContext _invocationContext;
    
    public ProjectStatusChangedHandler([WebhookParameter] ProjectStatusChangedRequest request, 
        [WebhookParameter] ProjectOptionalRequest project,
        InvocationContext invocationContext) : base(SubscriptionEvent)
    {
        _projectOptionalRequest = project;
        _projectStatusChangedRequest = request;
        _invocationContext = invocationContext;
    }
    
    public async Task<AfterSubscriptionEventResponse<ProjectDto>> OnWebhookSubscribedAsync()
    {
        if (_projectOptionalRequest.ProjectUId != null && _projectStatusChangedRequest.Status != null)
        {
            var client = new PhraseTmsClient(_invocationContext.AuthenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{_projectOptionalRequest.ProjectUId}", Method.Get,
                _invocationContext.AuthenticationCredentialsProviders);
            var project = await client.ExecuteWithHandling<ProjectDto>(request);
            
            if(project.Status == _projectStatusChangedRequest.Status)
            {
                return new AfterSubscriptionEventResponse<ProjectDto>()
                {
                    Result = project
                };
            }
            
            return new AfterSubscriptionEventResponse<ProjectDto>()
            {
                Result = null
            };
        }

        return new AfterSubscriptionEventResponse<ProjectDto>()
        {
            Result = null
        };
    }
}