using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class LockSegmentsFromMxliffRequest
{
    [Display("Bilingual file")]
    public FileReference File { get; set; } = default!;
}
