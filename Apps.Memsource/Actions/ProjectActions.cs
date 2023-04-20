using Apps.PhraseTms.Dtos;
using Apps.PhraseTms.Models.Projects.Requests;
using Apps.PhraseTms.Models.Projects.Responses;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Buffers.Text;
using System.ComponentModel;
using System.Text;

namespace Apps.PhraseTms.Actions
{
    [ActionList]
    public class ProjectActions
    {
        [Action("List all projects", Description = "List all projects")]
        public ListAllProjectsResponse ListAllProjects(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest("/api2/v1/projects", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<ResponseWrapper<List<ProjectDto>>>(request);
            return new ListAllProjectsResponse()
            {
                Projects = response.Content
            };
        }

        [Action("Get project", Description = "Get project by UId")]
        public ProjectDto GetProject(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetProjectRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Get, authenticationCredentialsProvider.Value);
            return client.Get<ProjectDto>(request);
        }

        [Action("Create project", Description = "Create project")]
        public ProjectDto CreateProject(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] CreateProjectRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest("/api2/v1/projects", Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                name = input.Name,
                sourceLang = input.SourceLanguage,
                targetLangs = input.TargetLanguages.ToArray()
            });
            return client.Post<ProjectDto>(request);
        }

        [Action("Create project from template", Description = "Create project from template")]
        public ProjectDto CreateProjectFromTemplate(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] CreateFromTemplateRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/applyTemplate/{input.TemplateUId}", Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                name = input.Name
            });
            return client.Post<ProjectDto>(request);
        }

        [Action("Add target language", Description = "Add target language")]
        public void AddTargetLanguage(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] AddTargetLanguageRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/targetLangs", Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                targetLangs = input.TargetLanguages.ToArray()
            });
            client.Execute(request);
        }

        [Action("Edit project", Description = "Edit project")]
        public void EditProject(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] EditProjectRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Patch, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                name = input.ProjectName,
                status = input.Status,
            });
            client.Execute(request);
        }

        [Action("Delete project", Description = "Delete project")]
        public void DeleteProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteProjectRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Delete, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
            client.Execute(request);
        }
    }
}
