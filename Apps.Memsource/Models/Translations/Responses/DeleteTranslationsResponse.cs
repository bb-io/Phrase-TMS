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
        [Display("Job ID")]
        public int Uid { get; set; }
    }
}
