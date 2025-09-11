using Apps.PhraseTMS.Models.Conversation;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Actions
{
    [ActionList("Conversations")]
    public class ConversationActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
    {

        //[Action("Get plain conversation")]
        //public async Task GetConversation([ActionParameter] ProjectRequest projectRequest, 
        //    [ActionParameter] GetConversationRequest conv)
        //{

        //    var endpoint = $"/api2/v1/jobs/{conv.JobUId}/conversations/plains/{conversationId}";
        //    var request = new RestRequest(endpoint, Method.Get);
        //    var response = await Client.ExecuteWithHandling<>(request);
        //    return response;
        //}
    }
}
