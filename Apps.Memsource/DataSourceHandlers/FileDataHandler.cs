using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers
{
    public class FileDataHandler : BaseInvocable, IAsyncDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

        public FileDataHandler(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var client = new PhraseTmsClient(Creds);
            var endpoint = "/api2/v1/files";
            var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
            var response = await client.Paginate<FileInfoDto>(request);

            return response
                .Where(x => context.SearchString == null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Take(20)
                .ToDictionary(x => x.UId, x => x.Name);
        }
        
    }
}
