using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS;
public class PhraseInvocable : BaseInvocable
{
    protected AuthenticationCredentialsProvider[] Creds =>
    InvocationContext.AuthenticationCredentialsProviders.ToArray();

    protected PhraseTmsClient Client { get; }
    public PhraseInvocable(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new PhraseTmsClient(invocationContext.AuthenticationCredentialsProviders);
    }
}
