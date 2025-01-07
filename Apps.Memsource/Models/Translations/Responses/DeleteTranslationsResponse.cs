using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Translations.Responses
{
    public class DeleteTranslationsResponse
    {
        [Display("Jobs")]
        public IEnumerable<DeletedTranslations> Jobs { get; set; }
    }

    public class DeletedTranslations
    {
        [Display("ID of deleted translations")]
        public int Uid { get; set; }
    }
}
