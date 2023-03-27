using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource.Connections
{
    public class BearerConnectionProvider : IConnectionProvider
    {
        public AuthenticationCredentialsProvider Create(IDictionary<string, string> connectionValues)
        {
            var credential = connectionValues.First(x => x.Key == "Authorization");
            return new AuthenticationCredentialsProvider(AuthenticationCredentialsRequestLocation.None, credential.Key, credential.Value);
        }

        public string ConnectionName => "OAuth";


        public IEnumerable<string> ConnectionProperties => new[] { "Authorization" };
    }
}
