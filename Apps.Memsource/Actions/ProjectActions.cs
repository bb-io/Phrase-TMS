using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.Projects.Responses;
using Apps.PhraseTMS.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class ProjectActions(IFileManagementClient fileManagementClient)
{
    [Action("List projects", Description = "List all projects")]
    public async Task<ListAllProjectsResponse> ListAllProjects(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListAllProjectsQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/projects";
        var request = new PhraseTmsRequest(QueryHelper.WithQuery(endpoint, query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<ProjectDto>(request);

        return new()
        {
            Projects = response ?? new List<ProjectDto>()
        };
    }

    [Action("Find project", Description = "Return first matching project")]
    public async Task<ProjectDto> FindProject(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListAllProjectsQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/projects";
        var request = new PhraseTmsRequest(QueryHelper.WithQuery(endpoint, query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<ProjectDto>(request);

        return response.FirstOrDefault();
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
    public async Task<ProjectDto> GetProject(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Get,
            authenticationCredentialsProviders);
        return client.ExecuteWithHandling<ProjectDto>(request).Result;
    }

    [Action("Create project", Description = "Create a new project")]
    public Task<ProjectDto> CreateProject(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] CreateProjectRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest("/api2/v1/projects", Method.Post, authenticationCredentialsProviders)
            .WithJsonBody(new
            {
                name = input.Name,
                sourceLang = input.SourceLanguage,
                targetLangs = input.TargetLanguages.ToArray(),
                dateDue = input.DateDue
            }, JsonConfig.DateSettings);

        return client.ExecuteWithHandling<ProjectDto>(request);
    }

    [Action("Create project from template", Description = "Create a new project from the specific template")]
    public Task<ProjectDto> CreateProjectFromTemplate(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] CreateFromTemplateRequest input)
    {
        if (String.IsNullOrEmpty(input.TemplateUId))
        { throw new PluginMisconfigurationException("Template ID cannot be empty"); }
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/applyTemplate/{input.TemplateUId}", Method.Post,
                authenticationCredentialsProviders)
            .WithJsonBody(new
            {
                name = input.Name,
                dateDue = input.DateDue
            }, JsonConfig.DateSettings);

        return client.ExecuteWithHandling<ProjectDto>(request);
    }

    [Action("Add target language", Description = "Add a target language to the project")]
    public Task AddTargetLanguage(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] AddTargetLanguageRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/targetLangs", Method.Post,
            authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            targetLangs = input.TargetLanguages.ToArray()
        });

        return client.ExecuteWithHandling(request);
    }

    [Action("Update project", Description = "Update project with specified details based on project ID")]
    public Task EditProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] EditProjectRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        
        var bodyDictionary = new Dictionary<string, object>
        {
            { "name", input.ProjectName},
            { "status", input.Status}
        };

        if (input.DueDate.HasValue)
        {
            bodyDictionary.Add("dateDue", input.DueDate);
        }

        if (input.DomainId != null)
        {
            bodyDictionary.Add("domain", new
            {
                id = input.DomainId
            });
        }
        
        if(input.SubdomainId != null)
        {
            bodyDictionary.Add("subDomain", new
            {
                id = input.SubdomainId
            });
        }
        
        if(input.ClientId != null)
        {
            bodyDictionary.Add("client", new
            {
                id = input.ClientId
            });
        }
        
        if(input.BusinessUnit != null)
        {
            bodyDictionary.Add("businessUnit", new
            {
                id = input.BusinessUnit
            });
        }

        if (input.OwnerId != null)
        {
            bodyDictionary.Add("owner", new
            {
                id = input.OwnerId
            });
        }

        var request = new PhraseTmsRequest($"/api2/v1/projects/{projectRequest.ProjectUId}", Method.Patch,
                authenticationCredentialsProviders)
            .WithJsonBody(bodyDictionary, JsonConfig.DateSettings);

        return client.ExecuteWithHandling(request);
    }

    [Action("Delete project", Description = "Delete specific project")]
    public Task DeleteProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] DeleteProjectRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}";

        if (input.Purge != null)
            endpoint += $"?purge={input.Purge}";

        var request = new PhraseTmsRequest(endpoint, Method.Delete,
            authenticationCredentialsProviders);

        return client.ExecuteWithHandling(request);
    }

    [Action("Download project original files", Description = "Download project source files")]
    public async Task<DownloadProjectFilesResponse> DownloadProjectOriginalFiles(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest input)
    {
        var jobActions = new JobActions(fileManagementClient);
        var credentialsProviders = authenticationCredentialsProviders as AuthenticationCredentialsProvider[] ??
                                   authenticationCredentialsProviders.ToArray();

        var jobs = await jobActions.ListAllJobs(credentialsProviders, input, new ListAllJobsQuery(), new JobStatusesRequest(), null);
        var files = new List<FileReference>();
        foreach (var job in jobs.Jobs)
        {
            var file = await jobActions.DownloadOriginalFile(credentialsProviders, input,
                new JobRequest { JobUId = job.Uid });
            files.Add(file.File);
        }

        return new() { Files = files };
    }

    [Action("Download project target files", Description = "Download project target files")]
    public async Task<DownloadProjectFilesResponse> DownloadProjectTargetFiles(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest input)
    {
        var jobActions = new JobActions(fileManagementClient);
        var credentialsProviders = authenticationCredentialsProviders as AuthenticationCredentialsProvider[] ??
                                   authenticationCredentialsProviders.ToArray();

        var jobs = await jobActions.ListAllJobs(credentialsProviders, input, new ListAllJobsQuery(), new JobStatusesRequest(), null);
        var files = new List<FileReference>();
        foreach (var job in jobs.Jobs)
        {
            var file = await jobActions.DownloadTargetFile(credentialsProviders, input,
                new JobRequest { JobUId = job.Uid });
            files.Add(file.File);
        }

        return new() { Files = files };
    }

    [Action("Assign providers from template", Description = "Assig providers to project or specific jobs from a template")]
    public async Task AssignFromTemplate(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] AssignFromTemplateRequest input)
    {

        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        if (input.JobsUIds is not null && input.JobsUIds.Any())
        {
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/applyTemplate/{input.TemplateUId}/assignProviders/forJobParts", Method.Post,
                authenticationCredentialsProviders);
            request.WithJsonBody(JsonConvert.SerializeObject(new
             {
                 jobs =
                    input.JobsUIds.Select(x =>
                    new
                    {
                        uid = x
                    }).ToArray()
                ,
             }));
            await client.ExecuteAsync(request);
        }
        else 
        {
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/applyTemplate/{input.TemplateUId}/assignProviders", Method.Post,
                authenticationCredentialsProviders);

            await client.ExecuteAsync(request);
        }
    }

    [Action("Find project termbase", Description = "Get the termbase linked to a project based on optional filters")]
    public async Task<TermbaseDto> FindProjectTermbase(
    IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
    [ActionParameter] FindProjectTermbaseRequest request)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var endpoint = $"/api2/v1/projects/{request.ProjectUId}/termBases";

        var apiRequest = new PhraseTmsRequest(endpoint, Method.Get, authenticationCredentialsProviders);
        var response = await client.ExecuteWithHandling<TermbaseResponse>(apiRequest);

        var termbases = response.TermBases;

        if (!string.IsNullOrEmpty(request.LanguageCode))
        {
            termbases = termbases.Where(tb => tb.TermBase.Langs.Contains(request.LanguageCode)).ToList();
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            termbases = termbases.Where(tb => tb.TermBase.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var matchingTermbase = termbases.FirstOrDefault();
        if (matchingTermbase == null)
        {
            throw new PluginMisconfigurationException("No matching termbase found for the given criteria.");
        }

        return matchingTermbase.TermBase;
    }

}