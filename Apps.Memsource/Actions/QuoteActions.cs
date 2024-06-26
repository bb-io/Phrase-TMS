﻿using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Quotes.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class QuoteActions
{
    [Action("Get quote", Description = "Get quote by UID")]
    public async Task<QuoteDto> GetQuote(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetQuoteRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/quotes/{input.QuoteUId}", Method.Get, authenticationCredentialsProviders);
        var response = await client.ExecuteWithHandling(request);
            
        return JsonConvert.DeserializeObject<QuoteDto>(response.Content);
    }

    [Action("Create quote", Description = "Create a new project quote")]
    public Task<QuoteDto> CreateQuote(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] CreateQuoteRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v2/quotes", Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            analyse = new { id = input.AnalyseUId },
            name = input.Name,
            priceList = new { id = input.PriceListUId },
            project = new { uid = projectRequest.ProjectUId }
        });
        return client.ExecuteWithHandling<QuoteDto>(request);
    }

    [Action("Delete quote", Description = "Delete specific quote")]
    public Task DeleteQuote(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetQuoteRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/quotes/{input.QuoteUId}", Method.Delete, authenticationCredentialsProviders);
            
        return client.ExecuteWithHandling(request);       
    }
}