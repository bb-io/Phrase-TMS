using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.CustomFields;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class JobCustomFieldOptionDataHandler : PhraseInvocable, IAsyncDataSourceItemHandler
{
    private JobSingleSelectCustomFieldRequest CustomFieldRequest { get; }

    public JobCustomFieldOptionDataHandler(InvocationContext invocationContext,
        [ActionParameter] JobSingleSelectCustomFieldRequest input) : base(invocationContext)
    {
        CustomFieldRequest = input;
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(CustomFieldRequest.FieldUId))
            throw new PluginMisconfigurationException("Please fill in Custom Field ID first");

        var request = new RestRequest($"api2/v1/customFields/{CustomFieldRequest.FieldUId}/options", Method.Get);
        if (!string.IsNullOrWhiteSpace(context.SearchString))
            request.AddQueryParameter("name", context.SearchString);

        var response = await Client.PaginateOnce<CustomFieldOptionDto>(request);
        return response.Select(x => new DataSourceItem(x.UId, x.Value));
    }
}
