using System.Net;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.Connections;

public class ConnectionValidator(InvocationContext invocationContext) : BaseInvocable(invocationContext), IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
    {
        try
        {
            var client = new PhraseTmsClient(authProviders);
            var request = new RestRequest("/api2/v1/auth/whoAmI");

            var response = await client.ExecuteWithoutHandlingAsync(request, cancellationToken);
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new()
                {
                    IsValid = false,
                    Message = "Unauthorized. Please check your API key."
                };
            }
            
            return new()
            {
                IsValid = true
            };
        }
        catch (Exception ex)
        {
            InvocationContext.Logger?.LogError($"[PhraseConnectionValidator] Exception occurred while validating connection: {ex.Message}", []);
            return new()
            {
                IsValid = false,
                Message = ex.Message
            };
        }
    }
}