﻿using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.PhraseTMS.Connections;

public class ConnectionValidator : IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
    {
        try
        {
            var client = new PhraseTmsClient(authProviders);
            var request = new RestRequest("/api2/v1/projects", Method.Get);

            var response = await client.ExecuteWithHandling(request);

            if (!response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsValid = false,
                    Message = response.ErrorMessage
                };
            }

            return new()
            {
                IsValid = true
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                IsValid = false,
                Message = ex.Message
            };
        }
    }
}