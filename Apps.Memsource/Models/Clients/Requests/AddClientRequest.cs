using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Clients.Requests
{
    public class AddClientRequest
    {
        public string Name { get; set; }

        [Display("External ID")]
        public string? ExternalId { get; set; }
        public string? Note { get; set; }
        
        [Display("Display note in project")]
        public bool? DisplayNoteInProject { get; set; }
    }
}
