using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.PhraseTMS.Models.Connectors.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.String;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class ConnectorActions
{
    [Action("List connectors", Description = "List all connectors")]
    public Task<ConnectorsResponseWrapper> ListConnectors(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListConnectorsQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/connectors";
        var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

        return client.ExecuteWithHandling<ConnectorsResponseWrapper>(request);
    }

    [Action("Get connector", Description = "Get connector by Id")]
    public Task<ConnectorDto> GetConnector(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetConnectorRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/connectors/{input.ConnectorUId}", Method.Get,
            authenticationCredentialsProviders);

        return client.ExecuteWithHandling<ConnectorDto>(request);
    }
}