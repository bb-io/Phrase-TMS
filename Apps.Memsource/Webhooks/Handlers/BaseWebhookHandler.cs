using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Webhooks.Handlers.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks.Handlers;

public class BaseWebhookHandler(InvocationContext invocationContext, string subEvent)
    : BaseInvocable(invocationContext), IWebhookEventHandler, IAsyncValidatableWebhookEventHandler
{
    private const string PayloadUrlKey = "payloadUrl";

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProvider);
        var request = new RestRequest("/api2/v2/webhooks", Method.Post);
        request.WithJsonBody(new
        {
            events = new[] { subEvent },
            url = values[PayloadUrlKey],
            name = subEvent
        });

        await client.ExecuteWithHandling(request);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {
        try
        {
            var subscription = await FindSubscriptionAsync(authenticationCredentialsProvider, values);
            if (subscription is null)
            {
                return;
            }

            var client = new PhraseTmsClient(authenticationCredentialsProvider);
            await client.ExecuteWithHandling(new RestRequest($"/api2/v2/webhooks/{subscription.UId}", Method.Delete));
        }
        catch (Exception e)
        {
            var payloadUrl = values.TryGetValue(PayloadUrlKey, out var value) ? value : "N/A";
            InvocationContext.Logger?.LogError(
                $"[PhraseTMSWebhookHandler] Failed to unsubscribe from webhook ({subEvent}): {e.Message}; " +
                $"Payload URL: {payloadUrl}", []);
            throw;
        }
    }

    public async Task<WebhookSubscriptionValidationResponse> ValidateSubscription(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {
        try
        {
            var subscription = await FindSubscriptionAsync(authenticationCredentialsProvider, values);

            if (subscription is null)
            {
                return Invalid($"The '{subEvent}' webhook subscription no longer exists in Phrase TMS. " +
                               "Recreate the Bird to subscribe again.");
            }

            if (!subscription.IsEnabled)
            {
                return Invalid($"The '{subEvent}' webhook subscription is disabled in Phrase TMS. " +
                               "Enable it in Phrase TMS or recreate the Bird.");
            }

            if (!subscription.Events.Contains(subEvent, StringComparer.OrdinalIgnoreCase))
            {
                return Invalid($"The webhook subscription in Phrase TMS no longer listens to the '{subEvent}' event. " +
                               "Recreate the Bird to subscribe again.");
            }

            return new WebhookSubscriptionValidationResponse { IsValid = true };
        }
        catch (Exception e)
        {
            return Invalid($"Could not verify the '{subEvent}' webhook subscription in Phrase TMS: {e.Message}");
        }
    }

    private async Task<WebhookDto?> FindSubscriptionAsync(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProvider);
        var request = new RestRequest($"/api2/v2/webhooks?name={subEvent}&url={values[PayloadUrlKey]}");
        var webhooks = await client.ExecuteWithHandling<ResponseWrapper<List<WebhookDto>>>(request);

        return webhooks?.Content?.FirstOrDefault(x =>
            string.Equals(x.Url, values[PayloadUrlKey], StringComparison.OrdinalIgnoreCase));
    }

    private static WebhookSubscriptionValidationResponse Invalid(string message)
        => new() { IsValid = false, Message = message };
}
