
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.PhraseTMS.Models.CustomFields;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class CustomFieldOptionDataHandler : PhraseInvocable, IAsyncDataSourceItemHandler
{
    private SingleSelectCustomFieldRequest customFieldRequest { get; set; }

    public CustomFieldOptionDataHandler(InvocationContext invocationContext, [ActionParameter] SingleSelectCustomFieldRequest input) : base(invocationContext)
    {
        customFieldRequest = input;
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(customFieldRequest.FieldUId))
        {
            throw new PluginMisconfigurationException("Please fill in Custom Field ID first");
        }
        
        var endpoint = $"api2/v1/customFields/{customFieldRequest.FieldUId}/options";
        var request = new RestRequest(endpoint, Method.Get);
        if (context.SearchString != null) request.AddQueryParameter("name", context.SearchString);
        var response = await Client.PaginateOnce<CustomFieldOptionDto>(request);
        return response.Select(x => new DataSourceItem(x.UId, x.Value));
    }
}

