using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Quotes.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.PhraseTMS.Models.Quotes.Responses;

namespace Apps.PhraseTMS.Actions;

[ActionList("Quotes")]
public class QuoteActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    [Action("Get quote", Description = "Get quote by ID")]
    public async Task<QuoteDto> GetQuote([ActionParameter] GetQuoteRequest input)
    {
        var request = new RestRequest($"/api2/v1/quotes/{input.QuoteUId}", Method.Get);
        var response = await Client.ExecuteWithHandling(request);
            
        return JsonConvert.DeserializeObject<QuoteDto>(response.Content);
    }

    [Action("Search quotes", Description = "List quotes for a specific project with optional name and status filtering")]
    public async Task<SearchQuotesResponse> SearchQuotes([ActionParameter] ProjectRequest project,
    [ActionParameter] SearchQuotesRequest input)
    {
        var request = new RestRequest($"/api2/v1/projects/{project.ProjectUId}/quotes", Method.Get);

        if (!string.IsNullOrEmpty(input.Name))
            request.AddQueryParameter("name", input.Name);

        var quotes = await Client.Paginate<QuoteDto>(request);

        if (!string.IsNullOrEmpty(input.Status))
            quotes = quotes
                .Where(q => string.Equals(q.Status, input.Status, StringComparison.OrdinalIgnoreCase))
                .ToList();

        return new SearchQuotesResponse { Quotes = quotes};
    }

    [Action("Find quote", Description = "Find a single quote in a project using filtering criteria")]
    public async Task<QuoteDto> FindQuote([ActionParameter] ProjectRequest project,
    [ActionParameter] FindQuoteRequest input)
    {
        var request = new RestRequest($"/api2/v1/projects/{project.ProjectUId}/quotes", Method.Get);

        if (!string.IsNullOrEmpty(input.Name))
            request.AddQueryParameter("name", input.Name);

        var quotes = await Client.Paginate<QuoteDto>(request);

        if (!string.IsNullOrEmpty(input.Status))
            quotes = quotes
                .Where(q => string.Equals(q.Status, input.Status, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (!string.IsNullOrEmpty(input.QuoteType))
            quotes = quotes
                .Where(q => string.Equals(q.QuoteType, input.QuoteType, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (!string.IsNullOrEmpty(input.NameContains))
            quotes = quotes
                .Where(q => q.Name.Contains(input.NameContains, StringComparison.OrdinalIgnoreCase))
                .ToList();

        return quotes.FirstOrDefault();
    }

    // Todo: add more inputs
    [Action("Create quote", Description = "Create a new project quote")]
    public Task<QuoteDto> CreateQuote([ActionParameter] ProjectRequest projectRequest, [ActionParameter] CreateQuoteRequest input)
    {
        var request = new RestRequest($"/api2/v2/quotes", Method.Post);
        request.WithJsonBody(new
        {
            analyse = new { id = input.AnalyseUId },
            name = input.Name,
            priceList = new { id = input.PriceListUId },
            project = new { uid = projectRequest.ProjectUId }
        });
        return Client.ExecuteWithHandling<QuoteDto>(request);
    }

    [Action("Delete quote", Description = "Delete specific quote")]
    public Task DeleteQuote([ActionParameter] GetQuoteRequest input)
    {
        var request = new RestRequest($"/api2/v1/quotes/{input.QuoteUId}", Method.Delete);            
        return Client.ExecuteWithHandling(request);       
    }
}