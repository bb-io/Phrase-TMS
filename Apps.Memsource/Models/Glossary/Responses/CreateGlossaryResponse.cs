using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Glossary.Responses;

public class CreateGlossaryResponse
{
    [Display("Glossary ID")]
    public string GlossaryId { get; set; }
}