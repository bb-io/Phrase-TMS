using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Responses
{
    public class ListReferenceFilesResponse
    {
        [Display("Reference file info")]
        public IEnumerable<ReferenceFileInfoDto> ReferenceFileInfo { get; set; }
    }
}
