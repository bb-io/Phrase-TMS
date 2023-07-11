using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTms.Dtos
{
    public class JobDto
    {
        [Display("Job ID")]
        public string UId { get; set; }
        public string Filename { get; set; }

        public string Status { get; set; }

        [Display("Target language")]
        public string TargetLang { get; set; }

        public ProjectDto Project { get; set; }

        //public string DateDue { get; set; }
    }
}
