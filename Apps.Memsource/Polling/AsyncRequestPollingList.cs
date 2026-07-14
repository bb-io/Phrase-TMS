using Apps.PhraseTMS.Dtos.Async;
using Apps.PhraseTMS.Models.AsyncRequests.Requests;
using Apps.PhraseTMS.Models.AsyncRequests.Responses;
using Apps.PhraseTMS.Polling.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.PhraseTMS.Polling;

[PollingEventList("Async requests")]
public class AsyncRequestPollingList(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    [PollingEvent("On async request finished", "Triggered when a Phrase TMS asynchronous request finishes")]
    public async Task<PollingEventResponse<PollingMemory, AsyncRequestStatusResponse>> OnAsyncRequestFinished(
        PollingEventRequest<PollingMemory> request,
        [PollingEventParameter] AsyncRequestIdRequest input)
    {
        var nowUtc = DateTime.UtcNow;
        var asyncRequest = await GetAsyncRequest(input.AsyncRequestId);
        var result = MapAsyncRequest(asyncRequest);

        return new PollingEventResponse<PollingMemory, AsyncRequestStatusResponse>
        {
            FlyBird = result.Finished,
            Result = result.Finished ? result : null,
            Memory = new PollingMemory
            {
                LastPollingTime = nowUtc
            }
        };
    }

    private async Task<AsyncRequest> GetAsyncRequest(string asyncRequestId)
    {
        var request = new RestRequest($"/api2/v1/async/{asyncRequestId}", Method.Get);
        return await Client.ExecuteWithHandling<AsyncRequest>(request);
    }

    private static AsyncRequestStatusResponse MapAsyncRequest(AsyncRequest asyncRequest)
        => new()
        {
            AsyncRequestId = asyncRequest.Id,
            Action = asyncRequest.Action,
            RequestedAt = asyncRequest.DateCreated,
            Finished = asyncRequest.AsyncResponse is not null,
            FinishedAt = asyncRequest.AsyncResponse?.DateCreated,
            ErrorCode = asyncRequest.AsyncResponse?.ErrorCode,
            ErrorDescription = asyncRequest.AsyncResponse?.ErrorDescription
        };
}
