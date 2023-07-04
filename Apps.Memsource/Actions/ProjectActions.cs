using Apps.PhraseTms.Dtos;
using Apps.PhraseTms.Models.Projects.Requests;
using Apps.PhraseTms.Models.Projects.Responses;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.PhraseTms.Actions
{
    [ActionList]
    public class ProjectActions
    {
        [Action("List all projects", Description = "List all projects")]
        public async Task<ListAllProjectsResponse> ListAllProjects(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest("/api2/v1/projects", Method.Get, authenticationCredentialsProviders);
            
            var response = await client.ExecuteWithHandling(()
                => client.ExecuteGetAsync<ResponseWrapper<List<ProjectDto>>>(request));
            
            return new ListAllProjectsResponse
            {
                Projects = response.Content
            };
        }

        [Action("Get project", Description = "Get project by UId")]
        public Task<ProjectDto> GetProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetProjectRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Get, authenticationCredentialsProviders);
            
            return client.ExecuteWithHandling(() => client.ExecuteGetAsync<ProjectDto>(request));
        }

        [Action("Create project", Description = "Create project")]
        public Task<ProjectDto> CreateProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] CreateProjectRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest("/api2/v1/projects", Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                name = input.Name,
                sourceLang = input.SourceLanguage,
                targetLangs = input.TargetLanguages.ToArray()
            });
            
            return client.ExecuteWithHandling(() => client.ExecutePostAsync<ProjectDto>(request));
        }

        [Action("Create project from template", Description = "Create project from template")]
        public Task<ProjectDto> CreateProjectFromTemplate(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] CreateFromTemplateRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/applyTemplate/{input.TemplateUId}", Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                name = input.Name
            });
            
            return client.ExecuteWithHandling(() => client.ExecutePostAsync<ProjectDto>(request));
        }

        [Action("Add target language", Description = "Add target language")]
        public Task AddTargetLanguage(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] AddTargetLanguageRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/targetLangs", Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                targetLangs = input.TargetLanguages.ToArray()
            });
            
            return client.ExecuteWithHandling(() => client.ExecuteAsync(request));
        }

        [Action("Edit project", Description = "Edit project")]
        public Task EditProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] EditProjectRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Patch, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                name = input.ProjectName,
                status = input.Status,
            });

            return client.ExecuteWithHandling(() => client.ExecuteAsync(request));
        }

        [Action("Delete project", Description = "Delete project")]
        public Task DeleteProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteProjectRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Delete, authenticationCredentialsProviders);
            
            return client.ExecuteWithHandling(() => client.ExecuteAsync(request));
        }
    }
}
