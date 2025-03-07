using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using DocumentFormat.OpenXml.Office.CoverPageProps;


namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class CreateJobsRequest
{
    [Display("Target languages")]
    [DataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string>? TargetLanguages { get; set; }

    [Display("Due date")]
    public DateTime? DueDate { get; set; }

    [Display("File")]
    public FileReference File { get; set; }

    [Display("Should the files be pre-translated?")]
    public bool? preTranslate { get; set; }

    [Display("Use project file import settings?")]
    public bool? useProjectFileImportSettings { get; set; }
}