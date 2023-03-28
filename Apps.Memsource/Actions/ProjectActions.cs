using Apps.Memsource.Dtos;
using Apps.Memsource.Models.Projects.Requests;
using Apps.Memsource.Models.Projects.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Buffers.Text;
using System.ComponentModel;
using System.Text;

namespace Apps.Memsource.Actions
{
    [ActionList]
    public class ProjectActions
    {
        [Action("List all projects", Description = "List all projects")]
        public ListAllProjectsResponse ListAllProjects(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest("/api2/v1/projects", Method.Get, "ApiToken " + authenticationCredentialsProvider.Value);
            var response = client.Get(request);
            dynamic content = JsonConvert.DeserializeObject(response.Content);
            JArray projectsArr = content.content;
            var projects = projectsArr.ToObject<List<ProjectDto>>();
            return new ListAllProjectsResponse()
            {
                Projects = projects
            };
        }

        [Action("Get project", Description = "Get project by UId")]
        public GetProjectResponse GetProject(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetProjectRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Get, "ApiToken " + authenticationCredentialsProvider.Value);
            var response = client.Get(request);
            JObject content = (JObject)JsonConvert.DeserializeObject(response.Content);
            var project = content.ToObject<ProjectDto>();
            return new GetProjectResponse()
            {
                Name = project.Name,
                Id = project.Id,
                DateCreated = project.DateCreated,
            };
        }

        [Action("Create project", Description = "Create project")]
        public void CreateProject(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] CreateProjectRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest("/api2/v1/projects", Method.Post, "ApiToken " + authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                name = input.Name,
                sourceLang = input.SourceLanguage,
                targetLangs = input.TargetLanguages.ToArray()
            });
            client.Execute(request);
        }

        [Action("Create project from template", Description = "Create project from template")]
        public void CreateProjectFromTemplate(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] CreateFromTemplateRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/applyTemplate/{input.TemplateUId}", Method.Post, "ApiToken " + authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                name = input.Name
            });
            client.Execute(request);
        }

        [Action("Add target language", Description = "Add target language")]
        public void AddTargetLanguage(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] AddTargetLanguageRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/{input.ProjectUId}/targetLangs", Method.Post, "ApiToken " + authenticationCredentialsProvider.Value);
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
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Patch, "ApiToken " + authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                name = input.ProjectName,
                status = input.Status,
            });
            client.Execute(request);
        }

        [Action("Delete project", Description = "Delete project")]
        public void DeleteProject(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] DeleteProjectRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Delete, "ApiToken " + authenticationCredentialsProvider.Value);
            client.Execute(request);
        }
    }
}
