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

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class TranslationActions
{
    [Action("List translation settings", Description = "List all machine translate settings")]
    public async Task<ListTranslationSettingsResponse> ListTranslationSettings(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListTranslationSettingsQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/machineTranslateSettings";
        var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get,
            authenticationCredentialsProviders);

        var response = await client.Paginate<TranslationSettingDto>(request);

        return new()
        {
            TranslationSettings = response
        };
    }

    [Action("Translate with MT", Description = "Translate with machine translation with custom settings")]
    public Task<TranslationDto> TranslateMT(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] [Display("MT settings UID")] string mtSettingsUId,
        [ActionParameter] TranslateMtRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/machineTranslations/{mtSettingsUId}/translate",
            Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(input,  JsonConfig.Settings);

        return client.ExecuteWithHandling<TranslationDto>(request);
    }

    [Action("Translate with MT by project", Description = "Translate with machine translation with project settings")]
    public Task<TranslationDto> TranslateMTProject(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] TranslateMtProjectRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest(
            $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/translations/translateWithMachineTranslation",
            Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            sourceTexts = input.SourceTexts.ToArray(),
        },  JsonConfig.Settings);
        return client.ExecuteWithHandling<TranslationDto>(request);
    }


    [Action("Delete all translations", Description = "Delete all translations by prject ID")]
    public async Task<DeleteTranslationsResponse> TranslateMTProject(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest(
            $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/translations",
            Method.Delete, authenticationCredentialsProviders);

        var response = await client.ExecuteWithHandling<DeleteTranslationsResponse>(request);

        return response;
    }

}