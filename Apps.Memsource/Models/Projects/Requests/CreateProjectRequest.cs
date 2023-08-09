using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }

        [Display("Source language")]
        [DataSource(typeof(LanguageDataHandler))]
        public string SourceLanguage { get; set; }

        [Display("Target languages")]
        public IEnumerable<string> TargetLanguages { get; set; }
    }
}
