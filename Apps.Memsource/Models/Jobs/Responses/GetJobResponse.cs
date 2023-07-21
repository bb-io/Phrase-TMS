using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class GetJobResponse
    {
        [Display("File name")]
        public string Filename { get; set; }

        public string Status { get; set; }

        [Display("Target language")]
        public string TargetLanguage { get; set; }

        [Display("Due date")]
        public string DateDue { get; set; }
    }
}
