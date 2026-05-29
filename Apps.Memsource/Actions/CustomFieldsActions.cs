using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.CustomFields;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System.Globalization;

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
        var field = projectCustomFields
        ?.FirstOrDefault(x => x?.customField?.uid == input.FieldUId);

        if (field == null)
            return null;

        var selectedValue = field.selectedOptions?
            .FirstOrDefault()?
            .value;

        return string.IsNullOrWhiteSpace(selectedValue) ? null : selectedValue;
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
            
            await Client.ExecuteWithHandling(request);
        }
        else 
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"value\": \"" + Value + "\"}]}";
            var request = new RestRequest(endpoint, Method.Post);
            request.AddStringBody(body, DataFormat.Json);
            await Client.ExecuteWithHandling(request);
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

            await Client.ExecuteWithHandling(request);
        }
        else
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"value\": " + Value + "}]}";
            var request = new RestRequest(endpoint, Method.Post);
            request.AddStringBody(body, DataFormat.Json);
            await Client.ExecuteWithHandling(request);
        }

    }

    [Action("Set project URL custom field value", Description = "Sets the URL value of a project custom field")]
    public async Task SetUrlCustomField([ActionParameter] ProjectRequest projectRequest,[ActionParameter] UrlCustomFieldRequest input,
        [ActionParameter] string Value)
    {
        var endpointList = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
        var requestList = new RestRequest(endpointList, Method.Get);
        var projectCustomFields = await Client.Paginate<ProjectCustomFieldDto>(requestList);

        if (projectCustomFields.Any(x => x.customField.uid == input.FieldUId))
        {
            var projectCustomField = projectCustomFields.First(x => x.customField.uid == input.FieldUId);
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/{projectCustomField.UId}";
            var body = new { value = Value };

            var request = new RestRequest(endpoint, Method.Put)
                .WithJsonBody(body, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    NullValueHandling = NullValueHandling.Ignore
                });

            await Client.ExecuteWithHandling(request);
        }
        else
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"value\": \"" + Value + "\"}]}";

            var request = new RestRequest(endpoint, Method.Post);
            request.AddStringBody(body, DataFormat.Json);

            await Client.ExecuteWithHandling(request);
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
                    value = dateString
                }
            }
            };
            var postRequest = new RestRequest(postEndpoint, Method.Post)
                .WithJsonBody(postBody);
            await Client.ExecuteWithHandling(postRequest);
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
            await Client.ExecuteWithHandling(request);
        }
        else
        {
            var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/customFields/";
            var body = "{\"customFieldInstances\": [{\"customField\":{\"uid\":\"" + input.FieldUId + "\"}, \"selectedOptions\": [{\"uid\": \""+Value.OptionUId+"\"}]}]}";
            var request = new RestRequest(endpoint, Method.Post);
            request.AddStringBody(body, DataFormat.Json);
            await Client.ExecuteWithHandling(request);
        }
    }

    [Action("Get job text custom field value", Description = "Gets the text value of a job custom field")]
    public async Task<string?> GetJobTextCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobTextCustomFieldRequest input)
    {
        var field = await GetJobCustomFieldInstance(projectRequest, jobRequest, input.FieldUId);
        return string.IsNullOrWhiteSpace(field?.Value) ? null : field.Value;
    }

    [Action("Get job numeric custom field value", Description = "Gets the value of a job custom field of type number")]
    public async Task<double?> GetJobNumberCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobNumberCustomFieldRequest input)
    {
        var field = await GetJobCustomFieldInstance(projectRequest, jobRequest, input.FieldUId);
        if (string.IsNullOrWhiteSpace(field?.Value))
            return null;

        return double.TryParse(field.Value, out var result) ? result : null;
    }

    [Action("Get job date custom field value", Description = "Gets the value of a job custom field of type date")]
    public async Task<DateTime?> GetJobDateCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobDateCustomFieldRequest input)
    {
        var field = await GetJobCustomFieldInstance(projectRequest, jobRequest, input.FieldUId);
        if (string.IsNullOrWhiteSpace(field?.Value))
            return null;

        return DateTime.TryParse(field.Value, out var result) ? result : null;
    }

    [Action("Get job single select custom field value", Description = "Gets the value of a job custom field of type single select")]
    public async Task<string?> GetJobSingleSelectCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobSingleSelectCustomFieldRequest input)
    {
        var field = await GetJobCustomFieldInstance(projectRequest, jobRequest, input.FieldUId);
        var selectedValue = field?.selectedOptions?.FirstOrDefault()?.value;
        return string.IsNullOrWhiteSpace(selectedValue) ? null : selectedValue;
    }

    [Action("Get job URL custom field value", Description = "Gets the URL value of a job custom field")]
    public async Task<string?> GetJobUrlCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobUrlCustomFieldRequest input)
    {
        var field = await GetJobCustomFieldInstance(projectRequest, jobRequest, input.FieldUId);
        return string.IsNullOrWhiteSpace(field?.Value) ? null : field.Value;
    }

    [Action("Get job multi select custom field value", Description = "Gets the value of a job custom field of type multi select")]
    public async Task<GetMultiSelectResponse> GetJobMultiSelectCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobMultiSelectCustomFieldRequest input)
    {
        var field = await GetJobCustomFieldInstance(projectRequest, jobRequest, input.FieldUId);
        return new GetMultiSelectResponse
        {
            Results = field?.selectedOptions?.Select(x => x.value) ?? Enumerable.Empty<string>()
        };
    }

    [Action("Set job text custom field value", Description = "Sets the text value of a job custom field")]
    public async Task SetJobTextCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobTextCustomFieldRequest input,
        [ActionParameter] string Value)
    {
        await UpsertJobCustomFieldValue(projectRequest, jobRequest, input.FieldUId, Value);
    }

    [Action("Set job numeric custom field value", Description = "Sets the number type value of a job custom field")]
    public async Task SetJobNumberCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobNumberCustomFieldRequest input,
        [ActionParameter] double Value)
    {
        await UpsertJobCustomFieldValue(projectRequest, jobRequest, input.FieldUId, Value);
    }

    [Action("Set job URL custom field value", Description = "Sets the URL value of a job custom field")]
    public async Task SetJobUrlCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobUrlCustomFieldRequest input,
        [ActionParameter] string Value)
    {
        await UpsertJobCustomFieldValue(projectRequest, jobRequest, input.FieldUId, Value);
    }

    [Action("Set job date custom field value", Description = "Sets the date type value of a job custom field")]
    public async Task SetJobDateCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobDateCustomFieldRequest input,
        [ActionParameter] DateTime Value)
    {
        var dateString = Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        await UpsertJobCustomFieldValue(projectRequest, jobRequest, input.FieldUId, dateString);
    }

    [Action("Set job single select custom field value", Description = "Sets the single select value of a job custom field")]
    public async Task SetJobSingleSelectCustomField([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] JobSingleSelectCustomFieldRequest input,
        [ActionParameter] JobSelectedOptionRequest value)
    {
        await UpsertJobCustomFieldSelectedOptions(projectRequest, jobRequest, input.FieldUId, [value.OptionUId]);
    }

    private async Task<ProjectCustomFieldDto?> GetJobCustomFieldInstance(ProjectRequest projectRequest,
        JobRequest jobRequest, string fieldUid)
    {
        ValidateJobCustomFieldRequest(projectRequest, jobRequest);

        if (string.IsNullOrWhiteSpace(fieldUid))
            throw new PluginMisconfigurationException("Field ID cannot be null or empty. Please check your input and try again");

        var customFields = await GetJobCustomFields(projectRequest.ProjectUId!, jobRequest.JobUId!);
        return customFields.FirstOrDefault(x => x.customField?.uid == fieldUid);
    }

    private async Task<List<ProjectCustomFieldDto>> GetJobCustomFields(string projectUid, string jobUid)
    {
        var request = new RestRequest($"/api2/v1/projects/{projectUid}/jobs/{jobUid}/customFields", Method.Get);
        return await Client.Paginate<ProjectCustomFieldDto>(request);
    }

    private async Task UpsertJobCustomFieldValue(ProjectRequest projectRequest, JobRequest jobRequest, string fieldUid,
        object value)
    {
        ValidateJobCustomFieldRequest(projectRequest, jobRequest);

        if (string.IsNullOrWhiteSpace(fieldUid))
            throw new PluginMisconfigurationException("Field ID cannot be null or empty. Please check your input and try again");

        var existing = await GetJobCustomFieldInstance(projectRequest, jobRequest, fieldUid);
        if (existing != null)
        {
            var request = new RestRequest(
                    $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}/customFields/{existing.UId}",
                    Method.Put)
                .WithJsonBody(new { value });

            await Client.ExecuteWithHandling(request);
            return;
        }

        var createRequest = new RestRequest(
                $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}/customFields",
                Method.Post)
            .WithJsonBody(new
            {
                customFieldInstances = new[]
                {
                    new
                    {
                        customField = new { uid = fieldUid },
                        value
                    }
                }
            });

        await Client.ExecuteWithHandling(createRequest);
    }

    private async Task UpsertJobCustomFieldSelectedOptions(ProjectRequest projectRequest, JobRequest jobRequest,
        string fieldUid, IEnumerable<string> optionUids)
    {
        ValidateJobCustomFieldRequest(projectRequest, jobRequest);

        if (string.IsNullOrWhiteSpace(fieldUid))
            throw new PluginMisconfigurationException("Field ID cannot be null or empty. Please check your input and try again");

        var selectedOptions = optionUids
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => new { uid = x })
            .ToArray();

        if (selectedOptions.Length == 0)
            throw new PluginMisconfigurationException("Selected option ID cannot be null or empty. Please check your input and try again");

        var existing = await GetJobCustomFieldInstance(projectRequest, jobRequest, fieldUid);
        if (existing != null)
        {
            var request = new RestRequest(
                    $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}/customFields/{existing.UId}",
                    Method.Put)
                .WithJsonBody(new { selectedOptions });

            await Client.ExecuteWithHandling(request);
            return;
        }

        var createRequest = new RestRequest(
                $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}/customFields",
                Method.Post)
            .WithJsonBody(new
            {
                customFieldInstances = new[]
                {
                    new
                    {
                        customField = new { uid = fieldUid },
                        selectedOptions
                    }
                }
            });

        await Client.ExecuteWithHandling(createRequest);
    }

    private static void ValidateJobCustomFieldRequest(ProjectRequest projectRequest, JobRequest jobRequest)
    {
        if (string.IsNullOrWhiteSpace(projectRequest?.ProjectUId))
            throw new PluginMisconfigurationException("Project ID cannot be null or empty. Please check your input and try again");

        if (string.IsNullOrWhiteSpace(jobRequest?.JobUId))
            throw new PluginMisconfigurationException("Job ID cannot be null or empty. Please check your input and try again");
    }
}
