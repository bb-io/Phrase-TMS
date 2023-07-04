using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.PhraseTMS.Models.Connectors.Requests;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class ConnectorActions
    {
        [Action("List all connectors", Description = "List all connectors")]
        public Task<ConnectorsResponseWrapper> ListConnectors(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/connectors", Method.Get, authenticationCredentialsProviders);
            
            return client.ExecuteWithHandling(() => client.ExecuteGetAsync<ConnectorsResponseWrapper>(request));
        }

        [Action("Get connector", Description = "Get connector by Id")]
        public Task<ConnectorDto> GetConnector(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetConnectorRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/connectors/{input.ConnectorUId}", Method.Get, authenticationCredentialsProviders);

            return client.ExecuteWithHandling(() => client.ExecuteGetAsync<ConnectorDto>(request));
        }
    }
}
