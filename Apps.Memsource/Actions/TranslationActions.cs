using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Translations.Requests;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Translations.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Actions;

[ActionList("Miscellaneous")]
public class TranslationActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    [Action("Delete all project translations", Description = "Delete all translations by project ID for the given jobs")]
    public Task DeleteAllTranslations([ActionParameter] ProjectRequest projectRequest, [ActionParameter] JobsInputRequest input)
    {
        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/translations", Method.Delete);

        var body = new
        {
            jobs = input.Jobs.Select(jobUid => new { uid = jobUid })
        };

        request.WithJsonBody(body, JsonConfig.Settings);

        return Client.ExecuteWithHandling<DeleteTranslationsResponse>(request);
    }

}