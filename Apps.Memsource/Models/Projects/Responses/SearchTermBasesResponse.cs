using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Responses
{
    public class SearchTermBasesResponse
    {
        [Display("Term bases")]
        public IEnumerable<TermbaseDto> TermBases { get; set; } = new List<TermbaseDto>();
    }
}
