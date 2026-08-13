using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class CreateJobsFromRemoteFileRequest
{
    [Display("Connector token")]
    [DataSource(typeof(ConnectorDataHandler))]
    public string ConnectorToken { get; set; } = string.Empty;

    [Display("Remote folder")]
    [FileDataSource(typeof(RemoteFolderDataHandler))]
    public string RemoteFolder { get; set; } = string.Empty;

    [Display("Remote file name")]
    [DataSource(typeof(RemoteFileDataHandler))]
    public string RemoteFileName { get; set; } = string.Empty;

    [Display("Target languages")]
    [DataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string>? TargetLanguages { get; set; }

    [Display("Due date")]
    public DateTime? DueDate { get; set; }

    [Display("Should the files be pre-translated?")]
    public bool? PreTranslate { get; set; }

    [Display("Use project file import settings?")]
    public bool? UseProjectFileImportSettings { get; set; }

    [Display("Continuous job")]
    public bool? Continuous { get; set; }
}
