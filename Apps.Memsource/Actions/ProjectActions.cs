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
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class ProjectActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Seach projects", Description = "Search for projects matching the filters of the input parameters")]
    public async Task<ListAllProjectsResponse> ListAllProjects([ActionParameter] ListAllProjectsQuery query)
    {
        var endpoint = "/api2/v1/projects";
        var request = new RestRequest(QueryHelper.WithQuery(endpoint, query), Method.Get);
        var response = await Client.Paginate<ProjectDto>(request);

        return new()
        {
            Projects = response ?? new List<ProjectDto>()
        };
    }

    [Action("Find project", Description = "Given the same parameters as 'Search projects', only returns the first matching project")]
    public async Task<ProjectDto> FindProject([ActionParameter] ListAllProjectsQuery query)
    {
        var endpoint = "/api2/v1/projects";
        var request = new RestRequest(QueryHelper.WithQuery(endpoint, query), Method.Get);

        var response = await Client.Paginate<ProjectDto>(request);

        return response.FirstOrDefault();
    }

    [Action("Get project", Description = "Get global project data for a specific project")]
    public async Task<ProjectDto> GetProject([ActionParameter] ProjectRequest input)
    {
        var request = new RestRequest($"/api2/v1/projects/{input.ProjectUId}", Method.Get);
        return await Client.ExecuteWithHandling<ProjectDto>(request);
    }

    [Action("Create project", Description = "Create a new project")]
    public Task<ProjectDto> CreateProject([ActionParameter] CreateProjectRequest input)
    {
        var request = new RestRequest("/api2/v3/projects", Method.Post)
            .WithJsonBody(new
            {
                name = input.Name,
                sourceLang = input.SourceLanguage,
                targetLangs = input.TargetLanguages.ToArray(),
                dateDue = input.DateDue,
                client = input.ClientId != null ? new { id = input.ClientId } : null,
                businessUnit = input.BusinessUnitId != null ? new { id = input.BusinessUnitId } : null,
                domain = input.DomainId != null ? new { id = input.DomainId } : null,
                subdomain = input.SubDomainId != null ? new { id = input.SubDomainId } : null,
                costCenter = input.CostCenterId != null ? new { id = input.CostCenterId } : null,
                purchaseOrder = input.PurchaseOrder,
                workflowSteps = input.WorkflowSteps?.Select(x=> new {id=x}),
                note = input.Note,
                fileHandover = input.FileHandover,
                propagateTranslationsToLowerWfDuringUpdateSource = input.PropagateTranslationsToLowerWfDuringUpdateSource
            }, JsonConfig.DateSettings);

        return Client.ExecuteWithHandling<ProjectDto>(request);
    }

    [Action("Create project from template", Description = "Create a new project from the specific template")]
    public Task<ProjectDto> CreateProjectFromTemplate([ActionParameter] CreateFromTemplateRequest input)
    {
        if (String.IsNullOrEmpty(input.TemplateUId))
        { throw new PluginMisconfigurationException("Template ID cannot be empty"); }
        var request = new RestRequest($"/api2/v2/projects/applyTemplate/{input.TemplateUId}", Method.Post)
            .WithJsonBody(new
            {
                name = input.Name,
                dateDue = input.DateDue,
                sourceLang = input.SourceLanguage,
                targetLangs = input.TargetLanguages?.ToArray(),
                workflowSteps = input.WorkflowSteps?.Select(x => new { id = x }),
                note = input.Note,
                client = input.ClientId != null ? new { id = input.ClientId } : null,
                businessUnit = input.BusinessUnitId != null ? new { id = input.BusinessUnitId } : null,
                domain = input.DomainId != null ? new { id = input.DomainId } : null,
                subdomain = input.SubDomainId != null ? new { id = input.SubDomainId } : null,
                costCenter = input.CostCenterId != null ? new { id = input.CostCenterId } : null,

            }, JsonConfig.DateSettings);

        return Client.ExecuteWithHandling<ProjectDto>(request);
    }

    [Action("Add project target language", Description = "Add a target language to the project")]
    public Task AddTargetLanguage([ActionParameter] ProjectRequest projectRequest, [ActionParameter] AddTargetLanguageRequest input)
    {
        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/targetLangs", Method.Post);
        request.WithJsonBody(new
        {
            targetLangs = input.TargetLanguages.ToArray()
        });

        return Client.ExecuteWithHandling(request);
    }

    [Action("Update project", Description = "Update project with specified details")]
    public Task EditProject([ActionParameter] ProjectRequest projectRequest, [ActionParameter] EditProjectRequest input)
    {        
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

        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}", Method.Patch)
            .WithJsonBody(bodyDictionary, JsonConfig.DateSettings);

        return Client.ExecuteWithHandling(request);
    }

    [Action("Delete project", Description = "Delete specific project")]
    public Task DeleteProject([ActionParameter] ProjectRequest projectRequest, [ActionParameter] DeleteProjectRequest input)
    {
        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}";

        if (input.Purge != null)
            endpoint += $"?purge={input.Purge}";

        var request = new RestRequest(endpoint, Method.Delete);
        return Client.ExecuteWithHandling(request);
    }

    [Action("Download project original files", Description = "Download project source files")]
    public async Task<DownloadProjectFilesResponse> DownloadProjectOriginalFiles([ActionParameter] ProjectRequest input)
    {
        var jobActions = new JobActions(InvocationContext, fileManagementClient);
        var jobs = await jobActions.ListAllJobs(input, new ListAllJobsQuery(), new JobStatusesRequest(), null);
        var files = new List<FileReference>();
        foreach (var job in jobs.Jobs)
        {
            var file = await jobActions.DownloadOriginalFile(input, new JobRequest { JobUId = job.Uid });
            files.Add(file.File);
        }

        return new() { Files = files };
    }

    [Action("Download project target files", Description = "Download project target files")]
    public async Task<DownloadProjectFilesResponse> DownloadProjectTargetFiles([ActionParameter] ProjectRequest input)
    {
        var jobActions = new JobActions(InvocationContext, fileManagementClient);
        var jobs = await jobActions.ListAllJobs(input, new ListAllJobsQuery(), new JobStatusesRequest(), null);
        var files = new List<FileReference>();
        foreach (var job in jobs.Jobs)
        {
            var file = await jobActions.DownloadTargetFile(input, new JobRequest { JobUId = job.Uid });
            files.Add(file.File);
        }

        return new() { Files = files };
    }

    [Action("Assign project providers from template", Description = "Assign providers to project or specific jobs from a template")]
    public async Task AssignFromTemplate([ActionParameter] AssignFromTemplateRequest input)
    {
        if (input.JobsUIds is not null && input.JobsUIds.Any())
        {
            var request = new RestRequest($"/api2/v1/projects/{input.ProjectUId}/applyTemplate/{input.TemplateUId}/assignProviders/forJobParts", Method.Post);
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
            await Client.ExecuteAsync(request);
        }
        else 
        {
            var request = new RestRequest($"/api2/v1/projects/{input.ProjectUId}/applyTemplate/{input.TemplateUId}/assignProviders", Method.Post);

            await Client.ExecuteAsync(request);
        }
    }

    [Action("Find project termbase", Description = "Get the termbase linked to a project based on optional filters")]
    public async Task<TermbaseDto> FindProjectTermbase([ActionParameter] FindProjectTermbaseRequest request)
    {
        var endpoint = $"/api2/v1/projects/{request.ProjectUId}/termBases";

        var apiRequest = new RestRequest(endpoint, Method.Get);
        var response = await Client.ExecuteWithHandling<TermbaseResponse>(apiRequest);

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