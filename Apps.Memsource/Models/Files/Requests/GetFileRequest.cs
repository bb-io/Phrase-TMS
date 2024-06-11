using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Files.Requests;

public class GetFileRequest
{
    [Display("File UID")]
    [DataSource(typeof(FileDataHandler))]
    public string FileUId { get; set; }
}