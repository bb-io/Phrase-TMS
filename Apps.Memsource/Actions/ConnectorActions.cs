using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Clients.Response;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Models.Connectors.Requests;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class ConnectorActions
    {
        [Action("List all connectors", Description = "List all connectors")]
        public ConnectorsResponseWrapper ListConnectors(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/connectors", Method.Get, authenticationCredentialsProvider.Value);
            return client.Get<ConnectorsResponseWrapper>(request);
        }

        [Action("Get connector", Description = "Get connector by Id")]
        public ConnectorDto GetConnector(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetConnectorRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/connectors/{input.ConnectorUId}", Method.Get, authenticationCredentialsProvider.Value);
            return client.Get<ConnectorDto>(request);
        }
    }
}
