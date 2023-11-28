using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class CreateJobRequest : ProjectRequest
{
    [Display("Target languages")]
    [DataSource(typeof(JobTargetLanguagesDataHandler))]
    public IEnumerable<string> TargetLanguages { get; set; }

    [Display("File")]
    public File File { get; set; }
        
}