using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class CreateProjectTemplateRequest
    {
        [Display("Template name")]
        public string Name { get; set; } = default!;

        [Display("Import settings ID")]
        public string? ImportSettingsUid { get; set; }

        [Display("Use dynamic title")]
        public bool? UseDynamicTitle { get; set; }

        [Display("Dynamic title")]
        public string? DynamicTitle { get; set; }
    }
}
