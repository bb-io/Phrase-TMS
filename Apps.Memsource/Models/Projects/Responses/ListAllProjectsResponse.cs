using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Projects.Responses
{
    public class ListAllProjectsResponse
    {
        public IEnumerable<ProjectDto> Projects { get; set; }
    }
}
