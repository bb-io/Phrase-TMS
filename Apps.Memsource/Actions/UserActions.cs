using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Users.Response;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Users.Requests;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class UserActions
    {
        [Action("List all users", Description = "List all users")]
        public async Task<ListAllUsersResponse> ListAllUsers(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/users", Method.Get, authenticationCredentialsProviders);
            
            var response = await client.ExecuteWithHandling(()
                => client.ExecuteGetAsync<ResponseWrapper<List<UserDto>>>(request));
            
            return new ListAllUsersResponse
            {
                Users = response.Content
            };
        }

        [Action("Get user", Description = "Get user by UId")]
        public Task<UserDto> GetUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetUserRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v3/users/{input.UserUId}",
                Method.Get, authenticationCredentialsProviders);
            
            return client.ExecuteWithHandling(() => client.ExecuteGetAsync<UserDto>(request));
        }

        [Action("Delete user", Description = "Delete user by UId")]
        public Task DeleteUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetUserRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/users/{input.UserUId}",
                Method.Delete, authenticationCredentialsProviders);
            
            return client.ExecuteWithHandling(() => client.ExecuteAsync(request));
        }
    }
}
