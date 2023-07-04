using Apps.PhraseTms.Dtos;

namespace Apps.PhraseTms.Models.Projects.Responses
{
    public class ListAllProjectsResponse
    {
        public IEnumerable<ProjectDto> Projects { get; set; }
    }
}
