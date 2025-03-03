using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Models.Jobs.Responses;

namespace Apps.PhraseTMS.Models.Responses;

public class JobResponseWrapper
{
    public IEnumerable<CreatedJobDto> Jobs { get; set; }
}