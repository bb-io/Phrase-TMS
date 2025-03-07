using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Users.Response;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Users.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class UserActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    [Action("Search users", Description = "Search through all users active on this Phrase instance")]
    public async Task<ListAllUsersResponse> ListAllUsers([ActionParameter] ListAllUsersQuery query)
    {
        var endpoint = "/api2/v1/users";
        var request = new RestRequest(endpoint.WithQuery(query), Method.Get);
        var response = await Client.Paginate<UserDto>(request);
        return new()
        {
            Users = response
        };
    }

    [Action("Find user", Description = "Given the search parameters, returns the first matching user")]
    public async Task<UserDto> FindUser([ActionParameter] ListAllUsersQuery query)
    {
        var endpoint = "/api2/v1/users";
        var request = new RestRequest(endpoint.WithQuery(query), Method.Get);
        var response = await Client.Paginate<UserDto>(request);
        return response.First();
    }

    [Action("Get user", Description = "Get user information by ID")]
    public Task<UserDto> GetUser( [ActionParameter] GetUserRequest input)
    {
        var request = new RestRequest($"/api2/v3/users/{input.UserUId}", Method.Get);
        return Client.ExecuteWithHandling<UserDto>(request);
    }

    [Action("Add user", Description = "Add a new user")]
    public Task<UserDto> AddUser([ActionParameter] AddUserRequest input)
    {
        var request = new RestRequest("/api2/v3/users", Method.Post);
        request.WithJsonBody(input, JsonConfig.Settings);

        return Client.ExecuteWithHandling<UserDto>(request);
    }
        
    [Action("Update user", Description = "Update a specific user")]
    public Task<UserDto> UpdateUser([ActionParameter] GetUserRequest user, [ActionParameter] AddUserRequest input)
    {
        var request = new RestRequest($"/api2/v3/users/{user.UserUId}", Method.Put);
        request.WithJsonBody(input, JsonConfig.Settings);
        return Client.ExecuteWithHandling<UserDto>(request);
    }

    [Action("Delete user", Description = "Delete a specific user")]
    public Task DeleteUser([ActionParameter] GetUserRequest input)
    {
        var request = new RestRequest($"/api2/v1/users/{input.UserUId}", Method.Delete);
        return Client.ExecuteWithHandling(request);
    }
}