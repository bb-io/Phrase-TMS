using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Debug;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
