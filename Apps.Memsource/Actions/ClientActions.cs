using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Clients.Response;
using Apps.PhraseTMS.Models.Clients.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class ClientActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    [Action("Search clients", Description = "Search through all clients")]
    public async Task<ListClientsResponse> ListClients([ActionParameter] ListClientsQuery query)
    {
        var endpoint = "/api2/v1/clients";
        var request = new RestRequest(endpoint.WithQuery(query), Method.Get);

        var response = await Client.Paginate<ClientDto>(request);

        return new()
        {
            Clients = response
        };
    }

    [Action("Get client", Description = "Get information about a specific client")]
    public Task<ClientDto> GetClient([ActionParameter] ClientRequest input)
    {
        var request = new RestRequest($"/api2/v1/clients/{input.ClientUid}", Method.Get);
        return Client.ExecuteWithHandling<ClientDto>(request);
    }

    [Action("Create client", Description = "Creates a new client")]
    public Task<ClientDto> AddClient([ActionParameter] AddClientRequest input)
    {
        var request = new RestRequest($"/api2/v1/clients", Method.Post);
        request.WithJsonBody(input, JsonConfig.Settings);

        return Client.ExecuteWithHandling<ClientDto>(request);
    }

    [Action("Update client", Description = "Update a specific client")]
    public Task<ClientDto> UpdateClient([ActionParameter] ClientRequest client, [ActionParameter] AddClientRequest input)
    {
        var request = new RestRequest($"/api2/v1/clients/{client.ClientUid}", Method.Put);
        request.WithJsonBody(input, JsonConfig.Settings);
        return Client.ExecuteWithHandling<ClientDto>(request);
    }

    [Action("Delete client", Description = "Delete a specific client")]
    public Task DeleteClient([ActionParameter] ClientRequest input)
    {
        var request = new RestRequest($"/api2/v1/clients/{input.ClientUid}", Method.Delete);
        return Client.ExecuteWithHandling(request);
    }
}