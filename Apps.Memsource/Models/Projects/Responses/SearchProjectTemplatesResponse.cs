using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Projects.Responses
{
    public class SearchProjectTemplatesResponse
    {
        public IEnumerable<ProjectTemplateDto> Templates { get; set; } = new List<ProjectTemplateDto>();
    }
}
