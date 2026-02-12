using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class ProjectActionsTests : TestBaseMultipleConnections
{
    public const string PROJECT_ID = "ayB1FFffK7hD0AXUAX9cPa";//"hGStrg0MLYmQtG0f66mj6f";

    [TestMethod, ContextDataSource]
    public async Task CreateProject_ValidData_Success(InvocationContext context)
    {
        var actions = new ProjectActions(context, FileManager);

        var targetLangs = new List<string>();
        targetLangs.Add("en");

        var workflow = new List<string>();
        workflow.Add("lxHYoo7KWvWN6R7Ca99mz3");
        workflow.Add("NeLxbybjmGHy69Dq0QAsR0");

        var input = new CreateProjectRequest { SourceLanguage = "hu", TargetLanguages = targetLangs, Name = "this name", WorkflowSteps = workflow};

        var result = await actions.CreateProject(input);

        PrintResult(result);
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.UId));
    }

    [TestMethod, ContextDataSource]
    public async Task CreateProjectFromTemplate_ValidData_Success(InvocationContext context)
    {
        var actions = new ProjectActions(context, FileManager);

        var targetLangs = new List<string>();
        targetLangs.Add("en");

        var workflow = new List<string>();
        workflow.Add("lxHYoo7KWvWN6R7Ca99mz3");
        workflow.Add("NeLxbybjmGHy69Dq0QAsR0");

        var input = new CreateFromTemplateRequest {SourceLanguage = "hu", 
            TargetLanguages=targetLangs,
            TemplateUId= "lG56tOurwL9u21kRlsXgy3", 
            Name="template project with date with neccesary timezone", 
        };

        var result = await actions.CreateProjectFromTemplate(input);

        PrintResult(result);
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.UId));
    }

    [TestMethod, ContextDataSource]
    public async Task Search_projects_works(InvocationContext context)
    {
        var actions = new ProjectActions(context, FileManager);
        var result = await actions.ListAllProjects(new ListAllProjectsQuery { CreatedInLastHours = 0.5});

        PrintResult(result);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Projects.Any());
    }

    [TestMethod, ContextDataSource]
    public async Task SearchTranslationMemories_works(InvocationContext context)
    {
        var actions = new TranslationMemoryActions(context, FileManager);
        var result = await actions.SearchTranslationMemories(new Apps.PhraseTMS.Models.TranslationMemories.Requests.SearchTranslationMemoryRequest { });

        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Get_project_works(InvocationContext context)
    {
        var actions = new ProjectActions(context, FileManager);
        var result = await actions.GetProject(new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" });

        PrintResult(result);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Name != null);
    }

    [TestMethod, ContextDataSource]
    public async Task EditProjectTranslationMemories_works(InvocationContext context)
    {
        var actions = new TranslationMemoryActions(context, FileManager);

        //var reqLangReadWrite = new Apps.PhraseTMS.Models.TranslationMemories.Requests.EditProjectTransMemoriesRequest
        //{
        //    TransMemoryUids = new[] { "w1pV1izYniDtTQjV4iPD1s" }
        //};

        var reqLangReadWrite = new Apps.PhraseTMS.Models.TranslationMemories.Requests.EditProjectTransMemoriesRequest
        {
            TargetLanguage = "de",
            TransMemoryUids = new[] { "w1pV1izYniDtTQjV4iPD1s" },
            ReadModes = new[] { true },
            WriteModes = new[] { true },
            Penalties = new[] { 2 },
            Orders = new[] { 1 },
        };

        var input = reqLangReadWrite;

        var result = await actions.EditProjectTranslationMemories(
            new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" },
            input
        );

        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Search_project_templates_works(InvocationContext context)
    {
        var actions = new ProjectTemplateActions(context);
        var result = await actions.SearchProjectTemplates(new SearchProjectTemplatesQuery { });

        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Search_termBase_templates_works(InvocationContext context)
    {
        var actions = new ProjectActions(context, FileManager);
        var result = await actions.SearchTermBases(new() { });

        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task CreateProjectTemplate_ValidData_Success(InvocationContext context)
    {
        var actions = new ProjectTemplateActions(context);
        var result = await actions.CreateProjectTemplate(new ProjectRequest { ProjectUId= "0SBo723Ge0wHfk0A1k1XWn0" },
            new CreateProjectTemplateRequest { Name="Testing template" });

        PrintResult(result);
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.UId));
    }

    [TestMethod, ContextDataSource]
    public async Task TemplateTermBasesProjectTemplate_ValidData_Success(InvocationContext context)
    {
        var actions = new ProjectTemplateActions(context);
        await actions.SetProjectTemplateTermBases(new ProjectTemplateRequest { ProjectTemplateUId = "hrNgeVe66AHkadtUCNjWm0" },
            new SetTemplateTermBasesRequest { TermBaseId= "EaZpWNsRTmbP9NEDxHlMl1", WorkflowStepId= "7445" });
    }

    [TestMethod, ContextDataSource]
    public async Task TemplateTranslationMemoryProjectTemplate_ValidData_Success(InvocationContext context)
    {
        var actions = new ProjectTemplateActions(context);
        await actions.SetProjectTemplateTranslationMemory(new ProjectTemplateRequest { ProjectTemplateUId = "hrNgeVe66AHkadtUCNjWm0" },
            new SetTemplateTranslationMemoryRequest { TransMemoryUid= "w1pV1izYniDtTQjV4iPD1s", WorkflowStepUid = "7446" });
    }

    [TestMethod, ContextDataSource]
    public async Task SetProjectTermBases_ValidData_Success(InvocationContext context)
    {
        var actions = new ProjectActions(context, FileManager);
        var result = await actions.SetProjectTermBases(new ProjectRequest { ProjectUId = "OQpwhSXsFoq5a9f4bVuuV0" },
            new SetProjectTermBasesRequest { ReadTermBaseIds= ["9N18Vm34tGRFb2Yia2EKR5"], WriteTermBaseId= "9N18Vm34tGRFb2Yia2EKR5", TargetLang = "de" });
        Console.WriteLine(JsonConvert.SerializeObject(result));
    }

    [TestMethod, ContextDataSource]
    public async Task SetProjectTranslationMemories_ValidData_Success(InvocationContext context)
    {
        var actions = new ProjectActions(context, FileManager);
        var result = await actions.SetProjectTranslationMemories(new ProjectRequest { ProjectUId = "OQpwhSXsFoq5a9f4bVuuV0" },
            new SetProjectTranslationMemoriesRequest { TranslationMemoryUids = ["w1pV1izYniDtTQjV4iPD1s"], TargetLang = "de" ,ReadMode = true,
                WriteMode = true
            });
        Console.WriteLine(JsonConvert.SerializeObject(result));
    }

}
