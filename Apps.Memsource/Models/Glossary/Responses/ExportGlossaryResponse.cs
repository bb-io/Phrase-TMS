using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Glossary.Response
{
    public class ExportGlossaryResponse
    {
        [Display("Glossary")]
        public FileReference File { get; set; }
    }
}
