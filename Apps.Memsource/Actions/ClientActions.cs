using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Extension;
using Apps.PhraseTMS.Models.Responses;
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
        [Action("List clients", Description = "List all clients")]
        public async Task<ListClientsResponse> ListClients(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListClientsQuery query)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);

            var endpoint = "/api2/v1/clients";
            var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

            var response = await client.Paginate<ClientDto>(request);

            return new ListClientsResponse
            {
                Clients = response
            };
        }

        [Action("Get client", Description = "Get specific client")]
        public Task<ClientDto> GetClient(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetClientRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/clients/{input.ClientUId}", Method.Get,
                authenticationCredentialsProviders);

            return client.ExecuteWithHandling<ClientDto>(request);
        }

        [Action("Add client", Description = "Add a new client")]
        public Task<ClientDto> AddClient(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] AddClientRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/clients", Method.Post, authenticationCredentialsProviders);
            request.WithJsonBody(input);
            
            return client.ExecuteWithHandling<ClientDto>(request);
        }

        [Action("Delete client", Description = "Delete specific client")]
        public Task DeleteClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetClientRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/clients/{input.ClientUId}", Method.Delete,
                authenticationCredentialsProviders);

            return client.ExecuteWithHandling(request);
        }
    }
}