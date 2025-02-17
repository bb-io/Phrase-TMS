using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos
{
    internal class CostCenterDto
    {
        public string Id { get; set; }

        public string UId { get; set; }

        public string Name { get; set; }

        [Display("Created by")]
        public UserDto CreatedBy { get; set; }
    }
}
