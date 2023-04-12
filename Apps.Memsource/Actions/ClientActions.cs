using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Models.Vendors.Response;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Models.Clients.Response;
using Apps.PhraseTMS.Models.Quotes.Request;
using Apps.PhraseTMS.Models.Clients.Requests;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class ClientActions
    {
        [Action("List all clients", Description = "List all clients")]
        public ListClientsResponse ListClients(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/clients", Method.Get, authenticationCredentialsProvider.Value);
            return new ListClientsResponse()
            {
                Clients = client.Get<ResponseWrapper<List<ClientDto>>>(request).Content
            };
        }

        [Action("Get client", Description = "Get client")]
        public ClientDto GetClient(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetClientRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/clients/{input.ClientUId}", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<ClientDto>(request);
            return response;
        }

        [Action("Add new client", Description = "Add new client")]
        public void AddClient(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] AddClientRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/clients", Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                name = input.Name,
                externalId = input.ExternalId,
            });
            client.Execute(request);
        }

        [Action("Delete client", Description = "Delete client")]
        public void DeleteClient(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetClientRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/clients/{input.ClientUId}", Method.Delete, authenticationCredentialsProvider.Value);
            client.Execute(request);
        }
    }
}
