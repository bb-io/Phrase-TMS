using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Conversations.Requests;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class ConversationTests : TestBaseMultipleConnections
{
    [TestMethod, ContextDataSource]
    public async Task Get_conversation_works(InvocationContext context)
    {
        // Arrange
        var actions = new ConversationActions(context);
        var conv = new ConversationRequest { ConversationUId = "8eb9ddb1_d052_4b76_b6c3_32d600a8e919" };
        var job = new JobRequest { JobUId = "ftRN9yMaryr4fRUYYbdX42" };
        var project = new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" };

        // Act
        var result = await actions.GetConversation(project, job, conv);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Search_conversations_works(InvocationContext context)
    {
        // Arrange
        var actions = new ConversationActions(context);
        var conv = new JobRequest { JobUId = "S1Lng7SgldQMeiwPm2srx3" };
        var project = new ProjectRequest { ProjectUId = "YWxQLsQXtwbN2FnxwoSFx0" };
        var search = new SearchConversationRequest
        {
            //IncludeDeleted = false,
            Since = DateTime.UtcNow.AddDays(-5)
        };

        // Act
        var result = await actions.SearchConversations(project, conv, search);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Delete_conversation_works(InvocationContext context)
    {
        // Arrange
        var actions = new ConversationActions(context);
        var conv = new ConversationRequest
        {
            ConversationUId = "8eb9ddb1_d052_4b76_b6c3_32d600a8e919"
        };
        var job = new JobRequest { JobUId = "ftRN9yMaryr4fRUYYbdX42" };
        var project = new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" };

        // Act
        await actions.DeleteConversation(project, job, conv);
    }

    [TestMethod, ContextDataSource]
    public async Task Edit_conversation_works(InvocationContext context)
    {
        // Arrange
        var actions = new ConversationActions(context);
        var conv = new ConversationRequest { ConversationUId = "8eb9ddb1_d052_4b76_b6c3_32d600a8e919" };
        var job = new JobRequest { JobUId = "ftRN9yMaryr4fRUYYbdX42" };
        var input = new EditConversationRequest { Status = "resolved" };
        var project = new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" };

        // Act
        var result = await actions.EditConversation(project, job, conv, input);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task CreateConversation_IsSuccess(InvocationContext context)
    {
        // Arrange
        var actions = new ConversationActions(context);
        var projectRequest = new ProjectRequest { ProjectUId = "YWxQLsQXtwbN2FnxwoSFx0" };
        var jobRequest = new JobRequest { JobUId = "S1Lng7SgldQMeiwPm2srx3" };
        var references = new ConversationReferencesRequest { SegmentId = "MXArLeioKmu7Bvcq_dc2:0" };
        var comment = new AddEditPlainCommentRequest { Text = "oi mate :)" };

        // Act
        var result = await actions.CreateConversation(projectRequest, jobRequest, references, comment);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task AddPlainComment_IsSuccess(InvocationContext context)
    {
        // Arrange
        var actions = new ConversationActions(context);
        var projectRequest = new ProjectRequest { ProjectUId = "YWxQLsQXtwbN2FnxwoSFx0" };
        var jobRequest = new JobRequest { JobUId = "S1Lng7SgldQMeiwPm2srx3" };
        var conversationRequest = new ConversationRequest { ConversationUId = "9ae3cc29_cfd8_47d9_b17e_c3a71781e1b8" };
        var input = new AddEditPlainCommentRequest { Text = "umm actually" };

        // Act
        var result = await actions.AddPlainComment(projectRequest, jobRequest, conversationRequest, input);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task EditPlainComment_IsSuccess(InvocationContext context)
    {
        // Arrange
        var actions = new ConversationActions(context);
        var projectRequest = new ProjectRequest { ProjectUId = "YWxQLsQXtwbN2FnxwoSFx0" };
        var jobRequest = new JobRequest { JobUId = "S1Lng7SgldQMeiwPm2srx3" };
        var conversationRequest = new ConversationRequest { ConversationUId = "9ae3cc29_cfd8_47d9_b17e_c3a71781e1b8" };
        var commentRequest = new CommentRequest { CommentUId = "1571be13_60a0_4173_a20c_084387d6d9ef" };
        var input = new AddEditPlainCommentRequest { Text = "hello world!" };

        // Act
        var result = await actions.EditPlainComment(projectRequest, jobRequest, conversationRequest, commentRequest, input);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task DeletePlainComment_IsSuccess(InvocationContext context)
    {
        // Arrange
        var actions = new ConversationActions(context);
        var projectRequest = new ProjectRequest { ProjectUId = "YWxQLsQXtwbN2FnxwoSFx0" };
        var jobRequest = new JobRequest { JobUId = "S1Lng7SgldQMeiwPm2srx3" };
        var conversationRequest = new ConversationRequest { ConversationUId = "9ae3cc29_cfd8_47d9_b17e_c3a71781e1b8" };
        var commentRequest = new CommentRequest { CommentUId = "1571be13_60a0_4173_a20c_084387d6d9ef" };

        // Act & Assert
        await actions.DeletePlainComment(projectRequest, jobRequest, conversationRequest, commentRequest);
    }
}
