using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos
{
    public class JobDto
    {
        [Display("UID")]
        public string Uid { get; set; }
        
        [Display("File name")]
        public string Filename { get; set; }

        public string Status { get; set; }

        [Display("Target language")]
        public string TargetLang { get; set; }

        [Display("Due date")]
        public string DateDue { get; set; }
    }
}
