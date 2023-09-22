using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.Projects.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class ProjectActions
{
    [Action("List projects", Description = "List all projects")]
    public async Task<ListAllProjectsResponse> ListAllProjects(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListAllProjectsQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/projects";
        var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<ProjectDto>(request);

        return new ListAllProjectsResponse
        {
            Projects = response
        };
    }
        
    [Action("List project templates", Description = "List all project templates")]
    public async Task<ListAllProjectTemplatesResponse> ListAllProjectTemplates(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
            
        var endpoint = "/api2/v1/projectTemplates";
        var request = new PhraseTmsRequest(endpoint, Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<ProjectTemplateDto>(request);

        return new(response);
    }

    [Action("Get project", Description = "Get project by UId")]
    public Task<ProjectDto> GetProject(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetProjectRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Get,
            authenticationCredentialsProviders);

        return client.ExecuteWithHandling<ProjectDto>(request);
    }

    [Action("Create project", Description = "Create a new project")]
    public Task<ProjectDto> CreateProject(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] CreateProjectRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest("/api2/v1/projects", Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            name = input.Name,
            sourceLang = input.SourceLanguage,
            targetLangs = input.TargetLanguages.ToArray()
        });

        return client.ExecuteWithHandling<ProjectDto>(request);
    }

    [Action("Create project from template", Description = "Create a new project from the specific template")]
    public Task<ProjectDto> CreateProjectFromTemplate(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] CreateFromTemplateRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/applyTemplate/{input.TemplateUId}", Method.Post,
            authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            name = input.Name
        });

        return client.ExecuteWithHandling<ProjectDto>(request);
    }

    [Action("Add target language", Description = "Add a target language to the project")]
    public Task AddTargetLanguage(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] AddTargetLanguageRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/targetLangs", Method.Post,
            authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            targetLangs = input.TargetLanguages.ToArray()
        });

        return client.ExecuteWithHandling(request);
    }

    [Action("Edit project", Description = "Edit selected project")]
    public Task EditProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] EditProjectRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Patch,
            authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            name = input.ProjectName,
            status = input.Status,
        });

        return client.ExecuteWithHandling(request);
    }

    [Action("Delete project", Description = "Delete specific project")]
    public Task DeleteProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] DeleteProjectRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = $"/api2/v1/projects/{input.ProjectUId}";

        if (input.Purge != null)
            endpoint += $"?purge={input.Purge}";
            
        var request = new PhraseTmsRequest(endpoint, Method.Delete,
            authenticationCredentialsProviders);

        return client.ExecuteWithHandling(request);
    }
}