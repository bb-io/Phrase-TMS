﻿using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Users.Response;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Users.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class UserActions
{
    [Action("List users", Description = "List all users")]
    public async Task<ListAllUsersResponse> ListAllUsers(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListAllUsersQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/users";
        var request =
            new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<UserDto>(request);

        return new()
        {
            Users = response
        };
    }

    [Action("Find user", Description = "Get first matching user")]
    public async Task<UserDto> FindUser(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListAllUsersQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/users";
        var request =
            new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<UserDto>(request);

        return response.First();
    }

    [Action("Get user", Description = "Get user by UId")]
    public Task<UserDto> GetUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetUserRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v3/users/{input.UserUId}",
            Method.Get, authenticationCredentialsProviders);

        return client.ExecuteWithHandling<UserDto>(request);
    }

    [Action("Add user", Description = "Add a new user")]
    public Task<UserDto> AddUser(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] AddUserRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest("/api2/v3/users",
            Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(input, JsonConfig.Settings);

        return client.ExecuteWithHandling<UserDto>(request);
    }
        
    [Action("Update user", Description = "Update specific user")]
    public Task<UserDto> UpdateUser(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetUserRequest user,
        [ActionParameter] AddUserRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v3/users/{user.UserUId}",
            Method.Put, authenticationCredentialsProviders);
        request.WithJsonBody(input, JsonConfig.Settings);

        return client.ExecuteWithHandling<UserDto>(request);
    }

    [Action("Delete user", Description = "Delete user by UId")]
    public Task DeleteUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetUserRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/users/{input.UserUId}",
            Method.Delete, authenticationCredentialsProviders);

        return client.ExecuteWithHandling(request);
    }
}