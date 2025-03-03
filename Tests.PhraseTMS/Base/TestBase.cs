using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace PhraseTMSTests.Base
{
    public class TestBase
    {
        public IEnumerable<AuthenticationCredentialsProvider> Creds { get; set; }

        public InvocationContext InvocationContext { get; set; }

        public FileManager FileManager { get; set; }

        public TestBase()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Creds = config.GetSection("ConnectionDefinition").GetChildren()
                .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value)).ToList();

            var password = Creds.First(x => x.KeyName == "password").Value;
            var userName = Creds.First(x => x.KeyName == "username").Value;
            var url = Creds.First(x => x.KeyName == "url").Value;

            var client = new RestClient(url.TrimEnd('/') + "/web");
            var request = new RestRequest("/api2/v3/auth/login", Method.Post);
            request.AddJsonBody(new { password, userName });
            var response = client.Execute<LoginResponse>(request);

            Creds = Creds.Append(new AuthenticationCredentialsProvider("Authorization", $"ApiToken {response.Data.Token}"));

            var relativePath = config.GetSection("TestFolder").Value;
            var projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            var folderLocation = Path.Combine(projectDirectory, relativePath);

            InvocationContext = new InvocationContext
            {
                AuthenticationCredentialsProviders = Creds,
            };

            FileManager = new FileManager();
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
