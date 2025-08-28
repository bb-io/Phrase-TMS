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