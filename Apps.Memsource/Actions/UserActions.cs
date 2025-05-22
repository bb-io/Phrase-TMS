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
        var request = new RestRequest("/api2/v1/users", Method.Get);

        if (!string.IsNullOrWhiteSpace(query.firstName))
            request.AddQueryParameter("firstName", query.firstName);
        if (!string.IsNullOrWhiteSpace(query.lastName))
            request.AddQueryParameter("lastName", query.lastName);
        if (!string.IsNullOrWhiteSpace(query.name))
            request.AddQueryParameter("name", query.name);
        if (!string.IsNullOrWhiteSpace(query.userName))
            request.AddQueryParameter("userName", query.userName);
        if (!string.IsNullOrWhiteSpace(query.email))
            request.AddQueryParameter("email", query.email);
        if (!string.IsNullOrWhiteSpace(query.nameOrEmail))
            request.AddQueryParameter("nameOrEmail", query.nameOrEmail);

        if (query.role?.Any() == true)
            foreach (var r in query.role)
                request.AddQueryParameter("role", r);

        if (query.includeDeleted == true)
            request.AddQueryParameter("includeDeleted", "true");

        if (query.sort?.Any() == true)
            foreach (var s in query.sort)
                request.AddQueryParameter("sort", s);

        if (query.order?.Any() == true)
            foreach (var o in query.order)
                request.AddQueryParameter("order", o);

        var users = await Client.Paginate<UserDto>(request);
        return new() { Users = users };
    }

    [Action("Find user", Description = "Given the search parameters, returns the first matching user")]
    public async Task<UserDto> FindUser([ActionParameter] ListAllUsersQuery query)
    {
        var request = new RestRequest("/api2/v1/users", Method.Get);

        if (!string.IsNullOrWhiteSpace(query.firstName))
            request.AddQueryParameter("firstName", query.firstName);
        if (!string.IsNullOrWhiteSpace(query.lastName))
            request.AddQueryParameter("lastName", query.lastName);
        if (!string.IsNullOrWhiteSpace(query.name))
            request.AddQueryParameter("name", query.name);
        if (!string.IsNullOrWhiteSpace(query.userName))
            request.AddQueryParameter("userName", query.userName);
        if (!string.IsNullOrWhiteSpace(query.email))
            request.AddQueryParameter("email", query.email);
        if (!string.IsNullOrWhiteSpace(query.nameOrEmail))
            request.AddQueryParameter("nameOrEmail", query.nameOrEmail);

        if (query.role?.Any() == true)
            foreach (var r in query.role)
                request.AddQueryParameter("role", r);

        if (query.includeDeleted == true)
            request.AddQueryParameter("includeDeleted", "true");

        if (query.sort?.Any() == true)
            foreach (var s in query.sort)
                request.AddQueryParameter("sort", s);

        if (query.order?.Any() == true)
            foreach (var o in query.order)
                request.AddQueryParameter("order", o);

        var users = await Client.Paginate<UserDto>(request);
        return users.First();
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