﻿using Apps.PhraseTMS.Models.Vendors.Requests;
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
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class TranslationActions
    {
        [Action("List translation settings", Description = "List translation settings")]
        public ListTranslationSettingsResponse ListTranslationSettings(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new PhraseTmsRequest($"/api2/v1/machineTranslateSettings", Method.Get, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
            var response = client.Get<ResponseWrapper<List<TranslationSettingDto>>>(request);
            return new ListTranslationSettingsResponse()
            {
                TranslationSettings = response.Content
            };
        }

        [Action("Translate with MT", Description = "Translate with MT with custom settings")]
        public TranslationDto TranslateMT(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TranslateMTRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new PhraseTmsRequest($"/api2/v1/machineTranslations/{input.MTSettingsUId}/translate", Method.Post, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
            request.AddJsonBody(new
            {
                from = input.SourceLanguageCode,
                to = input.TargetLanguageCode,
                sourceTexts = input.SourceTexts.ToArray(),
            });
            return client.Execute<TranslationDto>(request).Data;
        }

        [Action("Translate with MT by project", Description = "Translate with MT with project settings")]
        public TranslationDto TranslateMTProject(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TranslateMTProjectRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/translations/translateWithMachineTranslation", 
                Method.Post, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
            request.AddJsonBody(new
            {
                sourceTexts = input.SourceTexts.ToArray(),
            });
            return client.Execute<TranslationDto>(request).Data;
        }
    }
}
