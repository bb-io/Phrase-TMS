using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Responses
{
    public class ListReferenceFilesResponse
    {
        public IEnumerable<ReferenceFileInfoDto> ReferenceFileInfo { get; set; }
    }
}
