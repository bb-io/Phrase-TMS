using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Files.Responses
{
    public class ListAllFilesResponse
    {
        public IEnumerable<FileInfoDto> Files { get; set; }
    }
}
