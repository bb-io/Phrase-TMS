﻿
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.CustomFields;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class CustomFieldOptionDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private SingleSelectCustomFieldRequest customFieldRequest { get; set; }

    public CustomFieldOptionDataHandler(InvocationContext invocationContext, [ActionParameter] SingleSelectCustomFieldRequest input) : base(invocationContext)
    {
        customFieldRequest = input;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(customFieldRequest.FieldUId))
        {
            throw new ArgumentException("Please fill in Custom Field ID first");
        }
        var client = new PhraseTmsClient(Creds);

        var endpoint = $"api2/v1/customFields/{customFieldRequest.FieldUId}/options";
        var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
        var response = await client.Paginate<CustomFieldOptionDto>(request);
        return response
            .Where(x => context.SearchString == null ||
                        x.Value.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .ToDictionary(x => x.UId, x => x.Value);
    }
}
