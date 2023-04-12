using Apps.PhraseTms.Dtos;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Users.Response;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTms.Models.Jobs.Requests;
using Apps.PhraseTms.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Users.Requests;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class UserActions
    {
        [Action("List all users", Description = "List all users")]
        public ListAllUsersResponse ListAllUsers(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/users", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<ResponseWrapper<List<UserDto>>>(request);
            return new ListAllUsersResponse()
            {
                Users = response.Content
            };
        }

        [Action("Get user", Description = "Get user by UId")]
        public UserDto GetUser(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetUserRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v3/users/{input.UserUId}",
                Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<UserDto>(request);
            return response;
        }

        [Action("Delete user", Description = "Delete user by UId")]
        public void DeleteUser(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetUserRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/users/{input.UserUId}",
                Method.Delete, authenticationCredentialsProvider.Value);
            client.Execute(request);
        }
    }
}
