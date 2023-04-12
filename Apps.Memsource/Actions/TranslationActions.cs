using Apps.PhraseTMS.Models.Vendors.Requests;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Models.Translations.Requests;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Models.Translations.Responses;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class TranslationActions
    {
        [Action("List translation settings", Description = "List translation settings")]
        public ListTranslationSettingsResponse ListTranslationSettings(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/machineTranslateSettings", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<ResponseWrapper<List<TranslationSettingDto>>>(request);
            return new ListTranslationSettingsResponse()
            {
                TranslationSettings = response.Content
            };
        }

        [Action("Translate with MT", Description = "Translate with MT with custom settings")]
        public TranslationDto TranslateMT(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TranslateMTRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/machineTranslations/{input.MTSettingsUId}/translate", Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                from = input.SourceLanguageCode,
                to = input.TargetLanguageCode,
                sourceTexts = input.SourceTexts.ToArray(),
            });
            return client.Execute<TranslationDto>(request).Data;
        }

        [Action("Translate with MT by project", Description = "Translate with MT with project settings")]
        public TranslationDto TranslateMTProject(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TranslateMTProjectRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/translations/translateWithMachineTranslation", 
                Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                sourceTexts = input.SourceTexts.ToArray(),
            });
            return client.Execute<TranslationDto>(request).Data;
        }
    }
}
