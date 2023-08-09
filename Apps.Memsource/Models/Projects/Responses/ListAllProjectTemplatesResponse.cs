using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Projects.Responses;

public record ListAllProjectTemplatesResponse(List<ProjectTemplateDto> Templates);