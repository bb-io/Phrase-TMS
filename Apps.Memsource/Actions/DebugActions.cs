using Apps.PhraseTMS.Models.Debug;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Actions
{
    [ActionList("Miscellaneous")]
    public class DebugActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
    {
        [Action("Debug", Description = "Debug action")]
        public async Task<ResultResponse> GetAccessToken()
        {           
            return new()
            {
                Result = Creds.First(p => p.KeyName == "Authorization").Value
            };
        }
    }
}
