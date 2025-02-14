using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class UpdateSourceRequest
    {
        [Display("Job UIDs")]
        [DataSource(typeof(JobDataHandler))]
        public IEnumerable<JobUidModel> Jobs { get; set; }

        [Display("File")]
        public FileReference File { get; set; }

        [Display("Should the file be pre-translated?")]
        public bool? preTranslate { get; set; }
    }
}
