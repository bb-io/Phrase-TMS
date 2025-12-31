using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Models.Auth;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace PhraseTMSTests.Base;

public class TestBase
{
    public IEnumerable<AuthenticationCredentialsProvider> Creds { get; set; }

    public List<InvocationContext> InvocationContexts { get; set; }

    public List<IEnumerable<AuthenticationCredentialsProvider>> CredsGroups { get; set; }

    public InvocationContext InvocationContext { get; set; }

    public FileManager FileManager { get; set; }

    public TestContext? TestContext { get; set; }

    public TestBase()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        CredsGroups = config.GetSection("ConnectionDefinition")
            .GetChildren()
            .Select(section =>
                section.GetChildren().Select(child => new AuthenticationCredentialsProvider(child.Key, child.Value))
            )
            .ToList();

        var oauthGroup = CredsGroups.FirstOrDefault(group =>
            group.FirstOrDefault(c => c.KeyName == CredsNames.ConnectionType)?.Value == ConnectionTypes.OAuth2
        );

        if (oauthGroup != null)
        {
            var password = oauthGroup.First(x => x.KeyName == "password").Value;
            var userName = oauthGroup.First(x => x.KeyName == "username").Value;
            var url = oauthGroup.First(x => x.KeyName == "url").Value;

            var client = new RestClient(url.TrimEnd('/') + "/web");
            var request = new RestRequest("/api2/v3/auth/login", Method.Post);
            request.AddJsonBody(new { password, userName });

            var response = client.Execute<LoginResponse>(request);

            var tokenProvider = new AuthenticationCredentialsProvider("Authorization", $"ApiToken {response.Data.Token}");
            var newGroup = oauthGroup.Append(tokenProvider);

            var index = CredsGroups.IndexOf(oauthGroup);
            CredsGroups[index] = newGroup;
        }

        var relativePath = config.GetSection("TestFolder").Value;
        var projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        var folderLocation = Path.Combine(projectDirectory, relativePath);

        InitializeInvocationContext();

        FileManager = new FileManager();
    }

    private void InitializeInvocationContext()
    {
        InvocationContexts = new List<InvocationContext>();
        foreach (var credentialGroup in CredsGroups)
        {
            InvocationContexts.Add(new InvocationContext
            {
                AuthenticationCredentialsProviders = credentialGroup
            });
        }
    }

    public InvocationContext GetInvocationContext(string connectionType)
    {
        var context = InvocationContexts.FirstOrDefault(x => x.AuthenticationCredentialsProviders.Any(y => y.Value == connectionType));
        if (context == null)
            throw new Exception($"Invocation context was not found for this connection type: {connectionType}");
        else return context;
    }

    [Obsolete("Inherit from TestBaseMultipleConnections and use PrintResult instead")]
    public static void PrintResponse(object response)
    {
        Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
    }
}    
