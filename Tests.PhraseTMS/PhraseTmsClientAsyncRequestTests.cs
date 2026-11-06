using Apps.PhraseTMS;
using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Dtos.Analysis;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using PhraseTMSTests.Base;
using RestSharp;

namespace Tests.PhraseTMS;

[TestClass]
public class PhraseTmsClientAsyncRequestTests
{
    private static readonly TimeSpan TestGuard = TimeSpan.FromSeconds(20);

    [TestMethod]
    public async Task PerformAsyncRequest_request_never_finishes_throws_instead_of_waiting_forever()
    {
        using var server = new FakePhraseServer(request => request.HttpMethod == "POST"
            ? $"{{\"asyncRequest\":{FakePhraseServer.PendingAsyncRequestJson("async-1")}}}"
            : FakePhraseServer.PendingAsyncRequestJson("async-1"));
        var client = CreateClient(server.Url);

        var call = client.PerformAsyncRequest(new RestRequest("/api2/v2/transMemories/tm-1/export", Method.Post));

        var exception = await AssertFinishesWithException(call);
        StringAssert.Contains(exception.Message, "async-1");
    }

    [TestMethod]
    public async Task PerformMultipleAsyncRequest_request_never_finishes_throws_instead_of_waiting_forever()
    {
        using var server = new FakePhraseServer(request => request.HttpMethod == "POST"
            ? $"{{\"asyncRequests\":[{{\"asyncRequest\":{FakePhraseServer.PendingAsyncRequestJson("async-2")},\"analyse\":{{\"id\":\"analysis-1\"}}}}]}}"
            : FakePhraseServer.PendingAsyncRequestJson("async-2"));
        var client = CreateClient(server.Url);

        var call = client.PerformMultipleAsyncRequest<CreateAnalysisDto>(new RestRequest("/api2/v2/analyses", Method.Post));

        var exception = await AssertFinishesWithException(call);
        StringAssert.Contains(exception.Message, "async-2");
    }

    [TestMethod]
    public async Task PerformAsyncRequest_request_finishes_after_polling_returns_completed_request()
    {
        using var server = new FakePhraseServer(request => request.HttpMethod == "POST"
            ? $"{{\"asyncRequest\":{FakePhraseServer.PendingAsyncRequestJson("async-3")}}}"
            : FakePhraseServer.CompletedAsyncRequestJson("async-3"));
        var client = CreateClient(server.Url);

        var result = await client.PerformAsyncRequest(new RestRequest("/api2/v2/transMemories/tm-1/export", Method.Post));

        Assert.IsNotNull(result?.AsyncResponse);
        Assert.AreEqual("async-3", result.Id);
        Assert.AreEqual(1, server.AsyncStatusRequestCount);
    }

    private static PhraseTmsClient CreateClient(string url) => new(
    [
        new AuthenticationCredentialsProvider(CredsNames.Url, url),
        new AuthenticationCredentialsProvider(CredsNames.ConnectionType, ConnectionTypes.OAuth2),
        new AuthenticationCredentialsProvider("Authorization", "Bearer test-token")
    ])
    {
        AsyncRequestTimeout = TimeSpan.FromSeconds(1),
        AsyncRequestPollInterval = TimeSpan.FromMilliseconds(100)
    };

    private static async Task<PluginApplicationException> AssertFinishesWithException(Task call)
    {
        var finished = await Task.WhenAny(call, Task.Delay(TestGuard));
        Assert.AreSame(call, finished, "The client kept polling an unfinished Phrase async request.");
        return await Assert.ThrowsExceptionAsync<PluginApplicationException>(() => call);
    }
}
