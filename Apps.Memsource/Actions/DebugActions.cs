using Apps.PhraseTMS.Models.Debug;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class DebugActions
    {
        [Action("Get access token", Description = "Debug action")]
        public async Task<ResultResponse> GetAccessToken(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {           
            return new()
            {
                Result = authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value
            };
        }
    }
}
