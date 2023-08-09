using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.PriceLists.Response;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class PriceListActions
{
    [Action("List price lists", Description = "List all price lists")]
    public async Task<ListPriceListsResponse> ListPriceLists(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
            
        var endpoint = "/api2/v1/priceLists";
        var request = new PhraseTmsRequest(endpoint, Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<PriceListDto>(request);

        return new()
        {
            PriceLists = response
        };
    }
}