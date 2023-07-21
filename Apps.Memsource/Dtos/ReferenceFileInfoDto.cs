using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos
{
    public class ReferenceFileInfoDto
    {
        [Display("UID")]
        public string UId { get; set; }
        
        [Display("File name")]
        public string Filename { get; set; }
        public string Note { get; set; }
        //public string Id { get; set; }
    }
}
