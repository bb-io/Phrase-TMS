using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Clients.Response;
using Apps.PhraseTMS.Models.Clients.Requests;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class ClientActions
    {
        [Action("List all clients", Description = "List all clients")]
        public async Task<ListClientsResponse> ListClients(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/clients", Method.Get, authenticationCredentialsProviders);

            var response = await client.ExecuteWithHandling(() => client.ExecuteGetAsync<ResponseWrapper<List<ClientDto>>>(request));
            
            return new ListClientsResponse
            {
                Clients = response.Content
            };
        }

        [Action("Get client", Description = "Get client")]
        public Task<ClientDto> GetClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetClientRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/clients/{input.ClientUId}", Method.Get, authenticationCredentialsProviders);

            return client.ExecuteWithHandling(() => client.ExecuteGetAsync<ClientDto>(request));
        }

        [Action("Add new client", Description = "Add new client")]
        public Task<ClientDto> AddClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] AddClientRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/clients", Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                name = input.Name,
                externalId = input.ExternalId,
            });
            return client.ExecuteWithHandling(() => client.ExecutePostAsync<ClientDto>(request));
        }

        [Action("Delete client", Description = "Delete client")]
        public Task DeleteClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetClientRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/clients/{input.ClientUId}", Method.Delete, authenticationCredentialsProviders);

            return client.ExecuteWithHandling(() => client.ExecuteAsync(request));
        }
    }
}
