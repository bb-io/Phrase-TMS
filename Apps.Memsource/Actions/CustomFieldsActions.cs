using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.CustomFields;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RestSharp;
using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.PhraseTMS.Actions;

[ActionList("Custom fields")]
public class CustomFieldsActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    [Action("Get project text custom field value", Description = "Gets the text value of a project custom field")]
    public async Task<string> GetTextCustomField([ActionParameter] ProjectRequest projectRequest,[ActionParameter] TextCustomFieldRequest input)
    {
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new RestRequest(endpoint, Method.Get);
        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(request);
        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        { return projectCustomFields.FirstOrDefault(x => x.customField.uid == input.FieldUId).Value; }
        return null;
    }

    [Action("Get project numeric custom field value", Description = "Gets the value of a project custom field of type number")]
    public async Task<double?> GetNumberCustomField([ActionParameter] ProjectRequest projectRequest, [ActionParameter] NumberCustomFieldRequest input)
    {
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new RestRequest(endpoint, Method.Get);
        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(request);
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

    [Action("Get project project date custom field value", Description = "Gets the value of a project custom field of type date")]
    public async Task<DateTime?> GetDateCustomField([ActionParameter] ProjectRequest projectRequest, [ActionParameter] DateCustomFieldRequest input)
    {
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new RestRequest(endpoint, Method.Get);
        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(request);
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

    [Action("Get project single select custom field value", Description = "Gets the value of a project custom field of type single select")]
    public async Task<string?> GetSingleSelectCustomField([ActionParameter] ProjectRequest projectRequest, [ActionParameter] SingleSelectCustomFieldRequest input)
    {
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new RestRequest(endpoint, Method.Get);
        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(request);
        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            return projectCustomFields.FirstOrDefault(x => x.customField.uid == input.FieldUId).selectedOptions.First().value;
        }
        return null;
    }

    [Action("Get project URL custom field value", Description = "Gets the URL value of a project custom field")]
    public async Task<string?> GetUrlCustomField([ActionParameter] ProjectRequest projectRequest, [ActionParameter] UrlCustomFieldRequest input)
    {
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new RestRequest(endpoint, Method.Get);

        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(request);
        var instance = projectCustomFields.FirstOrDefault(x => x.customField?.uid == input.FieldUId);

        var value = instance?.Value;
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    [Action("Get project multi select custom field value", Description = "Gets the value of a project custom field of type multi select")]
    public async Task<GetMultiSelectResponse> GetMultiSelectCustomField([ActionParameter] ProjectRequest projectRequest, [ActionParameter] MultiSelectCustomFieldRequest input)
    {
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request = new RestRequest(endpoint, Method.Get);
        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(request);
        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            return new GetMultiSelectResponse { Results = projectCustomFields.FirstOrDefault(x => x.customField.uid == input.FieldUId).selectedOptions.Select(x => x.value)};
        }
        return new GetMultiSelectResponse();
    }

    [Action("Set project text custom field value", Description = "Sets the text value of a project custom field")]
    public async Task SetTextCustomField([ActionParameter] ProjectRequest projectRequest, [ActionParameter] TextCustomFieldRequest input, [ActionParameter] string Value)
    {
        var endpoint1 = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request1 = new RestRequest(endpoint1, Method.Get);
        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(request1);

        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var projectCustomField = projectCustomFields.First(x => x.customField.uid == input.FieldUId);
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{projectCustomField.UId}";
            var body = new { value = Value };
            var request = new RestRequest(endpoint, Method.Put)
                .WithJsonBody(body, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore
            }); ;
            
            var response = await Client.ExecuteAsync(request);
        }
        else 
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"value\": \"" + Value + "\"}]}";
            var request = new RestRequest(endpoint, Method.Post);
            request.AddStringBody(body, DataFormat.Json);
            var response = await Client.ExecuteAsync(request);
        }

    }

    [Action("Set project numeric custom field value", Description = "Sets the number type value of a project custom field")]
    public async Task SetNumberCustomField([ActionParameter] ProjectRequest projectRequest, [ActionParameter] NumberCustomFieldRequest input, [ActionParameter] double Value)
    {
        var endpoint1 = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request1 = new RestRequest(endpoint1, Method.Get);
        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(request1);

        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var projectCustomField = projectCustomFields.First(x => x.customField.uid == input.FieldUId);
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{projectCustomField.UId}";
            var body = new { value = Value };
            var request = new RestRequest(endpoint, Method.Put)
                .WithJsonBody(body, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    NullValueHandling = NullValueHandling.Ignore
                }); 

            var response = await Client.ExecuteAsync(request);
        }
        else
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"value\": " + Value + "}]}";
            var request = new RestRequest(endpoint, Method.Post);
            request.AddStringBody(body, DataFormat.Json);
            var response = await Client.ExecuteAsync(request);
        }

    }

    [Action("Set project URL custom field value", Description = "Sets the URL value of a project custom field")]
    public async Task SetUrlCustomField([ActionParameter] ProjectRequest projectRequest,[ActionParameter] UrlCustomFieldRequest input,
        [ActionParameter, Display("URL value")] string url)
    {

       if (!Uri.TryCreate(url, UriKind.Absolute, out _))
        throw new PluginMisconfigurationException("Please provide a valid absolute URL (e.g., https://example.com/path).");

        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore
        };

        var listEndpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields";
        var listRequest = new RestRequest(listEndpoint, Method.Get);
        var instances = await Client.Paginate<ProjectCustomFieldDto>(listRequest);

        var existing = instances.FirstOrDefault(x => x.customField?.uid == input.FieldUId);

        if (existing != null)
        {
            var putEndpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{existing.UId}";
            var putBody = new { value = url };

            var putRequest = new RestRequest(putEndpoint, Method.Put)
                .WithJsonBody(putBody, jsonSettings);
            await Client.ExecuteWithHandling(putRequest);
        }
        else
        {
            var postEndpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields";
            var postBody = new
            {
                customFieldInstances = new[]
                {
                    new
                    {
                        customField = new { uid = input.FieldUId },
                        value = url
                    }
                }
            };

            var postRequest = new RestRequest(postEndpoint, Method.Post)
                .WithJsonBody(postBody, jsonSettings);
            await Client.ExecuteWithHandling(postRequest);
        }
    }

    [Action("Set project date custom field value", Description = "Sets the date type value of a project custom field")]
    public async Task SetDateCustomField([ActionParameter] ProjectRequest projectRequest, [ActionParameter] DateCustomFieldRequest input, [ActionParameter] DateTime Value)
    {
        var listEndpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields";
        var listRequest = new RestRequest(listEndpoint, Method.Get);
        var instances = await Client.Paginate<ProjectCustomFieldDto>(listRequest);

        var dateString = Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

        var existing = instances.FirstOrDefault(x => x.customField?.uid == input.FieldUId);

        if (existing != null)
        {
            var putEndpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{existing.UId}";
            var putBody = new { value = dateString };
            var putRequest = new RestRequest(putEndpoint, Method.Put)
                .WithJsonBody(putBody);
            var putResponse = await Client.ExecuteWithHandling(putRequest);
        }
        else
        {
            var postEndpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields";
            var postBody = new
            {
                customFieldInstances = new[]
                {
                new
                {
                    customField = new { uid = input.FieldUId },
                    value = dateString
                }
            }
            };
            var postRequest = new RestRequest(postEndpoint, Method.Post)
                .WithJsonBody(postBody);
            var postResponse = await Client.ExecuteWithHandling(postRequest);
        }
    }


    [Action("Set project single select custom field value", Description = "Sets the single select value of a project custom field")]
    public async Task SetSingleSelectCustomField([ActionParameter] ProjectRequest projectRequest, [ActionParameter] SingleSelectCustomFieldRequest input, [ActionParameter] SelectedOptionRequest Value)
    {
        var endpoint1 = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var request1 = new RestRequest(endpoint1, Method.Get);
        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(request1);

        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var projectCustomField = projectCustomFields.First(x => x.customField.uid == input.FieldUId);
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{projectCustomField.UId}";
            var request = new RestRequest(endpoint, Method.Put);
            var body = "{\"selectedOptions\": [{\"uid\": \""+Value.OptionUId+"\"}]}";
            request.AddStringBody(body, DataFormat.Json); 
            var response = await Client.ExecuteAsync(request);
        }
        else
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"selectedOptions\": [{\"uid\": \""+Value.OptionUId+"\"}]}]}";
            var request = new RestRequest(endpoint, Method.Post);
            request.AddStringBody(body, DataFormat.Json);
            var response = await Client.ExecuteAsync(request);
        }

    }

}



