﻿using Apps.PhraseTms;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Webhooks.Handlers.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks.Handlers
{
    public class BaseWebhookHandler : IWebhookEventHandler
    {

        private string SubscriptionEvent;

        public BaseWebhookHandler(string subEvent)
        {
            SubscriptionEvent = subEvent;
        }

        public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
        {
            var authHeader = authenticationCredentialsProvider.First(p => p.KeyName == "Authorization").Value;
            var client = new PhraseTmsClient(authenticationCredentialsProvider);
            var request = new PhraseTmsRequest($"/api2/v2/webhooks", Method.Post, authenticationCredentialsProvider);
            request.AddJsonBody(new
            {
                events = new[] { SubscriptionEvent },
                url = values["payloadUrl"],
                name = SubscriptionEvent
            });
            await client.ExecuteAsync(request);
        }

        public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
        {
            var authHeader = authenticationCredentialsProvider.First(p => p.KeyName == "Authorization").Value;
            var client = new PhraseTmsClient(authenticationCredentialsProvider);
            var getRequest = new PhraseTmsRequest($"/api2/v2/webhooks?name={SubscriptionEvent}&url={values["payloadUrl"]}", Method.Get, authenticationCredentialsProvider);
            var webhooks = await client.GetAsync<ResponseWrapper<List<WebhookDto>>>(getRequest);
            var webhookUId = webhooks?.Content.FirstOrDefault()?.UId;

            if (webhookUId == null)
                return;

            var deleteRequest = new PhraseTmsRequest($"/api2/v2/webhooks/{webhookUId}", Method.Delete, authenticationCredentialsProvider);
            await client.ExecuteAsync(deleteRequest);
        }
    }
}
