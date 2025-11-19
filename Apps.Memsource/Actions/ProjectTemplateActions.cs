using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.Projects.Responses;
using Apps.PhraseTMS.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.PhraseTMS.Actions
{
    public class ProjectTemplateActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
    {
        [Action("Search project templates", Description = "Search for project templates matching the filters of the input parameters")]
        public async Task<SearchProjectTemplatesResponse> SearchProjectTemplates([ActionParameter] SearchProjectTemplatesQuery query)
        {
            var apiQuery = new SearchProjectTemplatesApiQuery(query);

            var request = new RestRequest(
                QueryHelper.WithQuery("/api2/v1/projectTemplates", apiQuery),
                Method.Get);

            var templates = await Client.Paginate<ProjectTemplateDto>(request);

            return new SearchProjectTemplatesResponse
            {
                Templates = templates ?? new List<ProjectTemplateDto>()
            };
        }

        [Action("Create project template", Description = "Create a project template from an existing project")]
        public async Task<ProjectTemplateDto> CreateProjectTemplate([ActionParameter] ProjectRequest projectRequest, [ActionParameter] CreateProjectTemplateRequest input)
        {
            if (string.IsNullOrWhiteSpace(projectRequest.ProjectUId))
            {
                throw new PluginMisconfigurationException(
                    "Project ID cannot be empty or null. Please check your input and try again.");
            }

            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new PluginMisconfigurationException(
                    "Template name cannot be empty. Please fill in the 'Template name' field.");
            }

            var body = new Dictionary<string, object>
            {
                ["project"] = new
                {
                    uid = projectRequest.ProjectUId
                },
                ["name"] = input.Name
            };

            if (!string.IsNullOrWhiteSpace(input.ImportSettingsUid))
            {
                body["importSettings"] = new
                {
                    uid = input.ImportSettingsUid
                };
            }

            if (input.UseDynamicTitle.HasValue)
            {
                body["useDynamicTitle"] = input.UseDynamicTitle.Value;
            }

            if (!string.IsNullOrWhiteSpace(input.DynamicTitle))
            {
                body["dynamicTitle"] = input.DynamicTitle;
            }

            var request = new RestRequest("/api2/v1/projectTemplates", Method.Post)
                .WithJsonBody(body, JsonConfig.DateSettings);

            return await Client.ExecuteWithHandling<ProjectTemplateDto>(request);
        }

        [Action("Set project template translation memory", Description = "Assign a translation memory to a project template for all target languages and workflow steps")]
        public async Task SetProjectTemplateTranslationMemory([ActionParameter] ProjectTemplateRequest templateRequest, [ActionParameter] SetTemplateTranslationMemoryRequest input)
        {
            if (string.IsNullOrWhiteSpace(templateRequest.ProjectTemplateUId))
            {
                throw new PluginMisconfigurationException(
                    "Project template UID cannot be empty or null. Please provide a valid template UID.");
            }

            if (string.IsNullOrWhiteSpace(input.TransMemoryUid))
            {
                throw new PluginMisconfigurationException(
                    "Translation memory UID cannot be empty. Please fill in the 'Translation memory UID' field.");
            }

            var getTemplateRequest = new RestRequest(
                $"/api2/v1/projectTemplates/{templateRequest.ProjectTemplateUId}",
                Method.Get);

            var template = await Client.ExecuteWithHandling<ProjectTemplateDto>(getTemplateRequest);

            if (template.TargetLangs == null || !template.TargetLangs.Any())
                throw new PluginMisconfigurationException(
                    "Project template has no target languages configured. Please add target languages in Phrase TMS.");

            if (template.WorkflowSteps == null || !template.WorkflowSteps.Any())
                throw new PluginMisconfigurationException(
                    "Project template has no workflow steps configured. Please add workflow steps in Phrase TMS.");

            var dataPerContext = template.TargetLangs
                .SelectMany(targetLang => template.WorkflowSteps.Select(step => new
                {
                    transMemories = new[]
                    {
                new
                {
                    transMemory = new { uid = input.TransMemoryUid },
                    readMode = true,
                    writeMode = true,
                }
                    },
                    targetLang = targetLang,
                    workflowStep = new { uid = step.Uid },
                    orderEnabled = false
                }))
                .ToArray();

            var endpoint = $"/api2/v2/projectTemplates/{templateRequest.ProjectTemplateUId}/transMemories";

            var body = new
            {
                dataPerContext
            };

            var request = new RestRequest(endpoint, Method.Put)
                .WithJsonBody(body, JsonConfig.DateSettings);

            await Client.ExecuteWithHandling(request);
        }

        [Action("Set project template term bases",Description = "Assign a term base to a project template for a specific target language and workflow step")]
        public async Task SetProjectTemplateTermBases([ActionParameter] ProjectTemplateRequest templateRequest, [ActionParameter] SetTemplateTermBasesRequest input)
        {
            if (string.IsNullOrWhiteSpace(templateRequest.ProjectTemplateUId))
            {
                throw new PluginMisconfigurationException(
                    "Project template UID cannot be empty or null. Please provide a valid template UID.");
            }

            if (string.IsNullOrWhiteSpace(input.TermBaseId))
            {
                throw new PluginMisconfigurationException(
                    "Term base ID cannot be empty. Please fill in the 'Term base ID' field.");
            }

            if (string.IsNullOrWhiteSpace(input.TargetLang))
            {
                throw new PluginMisconfigurationException(
                    "Target language cannot be empty. Please fill in the 'Target language' field.");
            }

            if (string.IsNullOrWhiteSpace(input.WorkflowStepId))
            {
                throw new PluginMisconfigurationException(
                    "Workflow step ID cannot be empty. Please fill in the 'Workflow step ID' field.");
            }

            var endpoint = $"/api2/v1/projectTemplates/{templateRequest.ProjectTemplateUId}/termBases";

            var body = new
            {
                readTermBases = new[]
                {
                new { id = input.TermBaseId }
            },
                writeTermBase = new
                {
                    id = input.TermBaseId
                },
                qualityAssuranceTermBases = new[]
                {
                new { id = input.TermBaseId }
            },
                targetLang = input.TargetLang,
                workflowStep = new
                {
                    id = input.WorkflowStepId
                }
            };

            var request = new RestRequest(endpoint, Method.Put)
                .WithJsonBody(body, JsonConfig.DateSettings);

            await Client.ExecuteWithHandling(request);
        }
    }
}
