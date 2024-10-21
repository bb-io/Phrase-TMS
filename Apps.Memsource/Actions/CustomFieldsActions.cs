using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.CustomFields;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RestSharp;
using Apps.PhraseTMS.Constants;

namespace Apps.PhraseTMS.Actions;

[ActionList]

public class CustomFieldsActions
{
    [Action("Get text custom field value", Description = "Gets the text value of a project custom field")]
    public async Task<string> GetTextCustomField(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] TextCustomFieldRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new PhraseTmsRequest(endpoint, Method.Get, authenticationCredentialsProviders);
        var projectCustomFields = await client.Paginate<ProjectCustomFieldDto>(request);
        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        { return projectCustomFields.FirstOrDefault(x => x.customField.uid == input.FieldUId).Value; }
        return null;
    }

    [Action("Get numeric custom field value", Description = "Gets the value of a project custom field of type number")]
    public async Task<double?> GetNumberCustomField(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] NumberCustomFieldRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new PhraseTmsRequest(endpoint, Method.Get, authenticationCredentialsProviders);
        var projectCustomFields = await client.Paginate<ProjectCustomFieldDto>(request);
        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var content = projectCustomFields.FirstOrDefault(x => x.customField.uid == input.FieldUId).Value;
            if (content is null) {  return null; }
            if (double.TryParse(content, out double result))
            {
                return result;
            }
        }
        return null;
    }

    [Action("Get date custom field value", Description = "Gets the value of a project custom field of type date")]
    public async Task<DateTime?> GetDateCustomField(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] DateCustomFieldRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new PhraseTmsRequest(endpoint, Method.Get, authenticationCredentialsProviders);
        var projectCustomFields = await client.Paginate<ProjectCustomFieldDto>(request);
        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var content = projectCustomFields.FirstOrDefault(x => x.customField.uid == input.FieldUId).Value;
            if (content is null) { return null; }
            if (DateTime.TryParse(content, out DateTime result))
            {
                return result;
            }
        }
        return null;
    }

    [Action("Get single select custom field value", Description = "Gets the value of a project custom field of type single select")]
    public async Task<string?> GetSingleSelectCustomField(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] SingleSelectCustomFieldRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new PhraseTmsRequest(endpoint, Method.Get, authenticationCredentialsProviders);
        var projectCustomFields = await client.Paginate<ProjectCustomFieldDto>(request);
        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            return projectCustomFields.FirstOrDefault(x => x.customField.uid == input.FieldUId).selectedOptions.First().value;
        }
        return null;
    }

    [Action("Get multi select custom field value", Description = "Gets the value of a project custom field of type multi select")]
    public async Task<GetMultiSelectResponse> GetMultiSelectCustomField(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] MultiSelectCustomFieldRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new PhraseTmsRequest(endpoint, Method.Get, authenticationCredentialsProviders);
        var projectCustomFields = await client.Paginate<ProjectCustomFieldDto>(request);
        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            return new GetMultiSelectResponse { Results = projectCustomFields.FirstOrDefault(x => x.customField.uid == input.FieldUId).selectedOptions.Select(x => x.value)};
        }
        return new GetMultiSelectResponse();
    }

    [Action("Set text custom field value", Description = "Sets the text value of a project custom field")]
    public async Task SetTextCustomField(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ProjectRequest projectRequest,
            [ActionParameter] TextCustomFieldRequest input, [ActionParameter] string Value)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint1 = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request1 = new PhraseTmsRequest(endpoint1, Method.Get, authenticationCredentialsProviders);
        var projectCustomFields = await client.Paginate<ProjectCustomFieldDto>(request1);

        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var client1 = new PhraseTmsClient(authenticationCredentialsProviders);
            var projectCustomField = projectCustomFields.First(x => x.customField.uid == input.FieldUId);
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{projectCustomField.UId}";
            var body = new { value = Value };
            var request = new PhraseTmsRequest(endpoint, Method.Put, authenticationCredentialsProviders)
                .WithJsonBody(body, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore
            }); ;
            
            var response = await client1.ExecuteAsync(request);
        }
        else 
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"value\": \"" + Value + "\"}]}";
            var request = new PhraseTmsRequest(endpoint, Method.Post, authenticationCredentialsProviders);
            request.AddStringBody(body, DataFormat.Json);
            var response = await client.ExecuteAsync(request);
        }

    }

    [Action("Set numeric custom field value", Description = "Sets the number type value of a project custom field")]
    public async Task SetNumberCustomField(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ProjectRequest projectRequest,
            [ActionParameter] NumberCustomFieldRequest input, [ActionParameter] double Value)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint1 = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request1 = new PhraseTmsRequest(endpoint1, Method.Get, authenticationCredentialsProviders);
        var projectCustomFields = await client.Paginate<ProjectCustomFieldDto>(request1);

        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var client1 = new PhraseTmsClient(authenticationCredentialsProviders);
            var projectCustomField = projectCustomFields.First(x => x.customField.uid == input.FieldUId);
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{projectCustomField.UId}";
            var body = new { value = Value };
            var request = new PhraseTmsRequest(endpoint, Method.Put, authenticationCredentialsProviders)
                .WithJsonBody(body, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    NullValueHandling = NullValueHandling.Ignore
                }); 

            var response = await client1.ExecuteAsync(request);
        }
        else
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"value\": " + Value + "}]}";
            var request = new PhraseTmsRequest(endpoint, Method.Post, authenticationCredentialsProviders);
            request.AddStringBody(body, DataFormat.Json);
            var response = await client.ExecuteAsync(request);
        }

    }

    [Action("Set date custom field value", Description = "Sets the date type value of a project custom field")]
    public async Task SetDateCustomField(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ProjectRequest projectRequest,
            [ActionParameter] DateCustomFieldRequest input, [ActionParameter] DateTime Value)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint1 = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request1 = new PhraseTmsRequest(endpoint1, Method.Get, authenticationCredentialsProviders);
        var projectCustomFields = await client.Paginate<ProjectCustomFieldDto>(request1);

        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var client1 = new PhraseTmsClient(authenticationCredentialsProviders);
            var projectCustomField = projectCustomFields.First(x => x.customField.uid == input.FieldUId);
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{projectCustomField.UId}";
            var body = new { value = Value };
            var request = new PhraseTmsRequest(endpoint, Method.Put, authenticationCredentialsProviders)
                .WithJsonBody(body,JsonConfig.DateSettings);
            var response = await client1.ExecuteAsync(request);
        }
        else
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"value\": \""+Value+"\"}]}";
            var request = new PhraseTmsRequest(endpoint, Method.Post, authenticationCredentialsProviders)
            .WithJsonBody(body, JsonConfig.DateSettings);
            var response = await client.ExecuteAsync(request);
        }

    }

    [Action("Set single select custom field value", Description = "Sets the single select value of a project custom field")]
    public async Task SetSingleSelectCustomField(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] SingleSelectCustomFieldRequest input, [ActionParameter] SelectedOptionRequest Value)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint1 = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request1 = new PhraseTmsRequest(endpoint1, Method.Get, authenticationCredentialsProviders);
        var projectCustomFields = await client.Paginate<ProjectCustomFieldDto>(request1);

        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var projectCustomField = projectCustomFields.First(x => x.customField.uid == input.FieldUId);
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{projectCustomField.UId}";
            var request = new PhraseTmsRequest(endpoint, Method.Put, authenticationCredentialsProviders);
            var body = "{\"selectedOptions\": [{\"uid\": \""+Value.OptionUId+"\"}]}";
            request.AddStringBody(body, DataFormat.Json); 
            var response = await client.ExecuteAsync(request);
        }
        else
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"selectedOptions\": [{\"uid\": \""+Value.OptionUId+"\"}]}]}";
            var request = new PhraseTmsRequest(endpoint, Method.Post, authenticationCredentialsProviders);
            request.AddStringBody(body, DataFormat.Json);
            var response = await client.ExecuteAsync(request);
        }

    }

}



