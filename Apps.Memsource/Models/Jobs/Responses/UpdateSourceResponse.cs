using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class UpdateSourceResponse
    {
        [Display("Job ID")] public string Uid { get; set; }

        [Display("File name")] public string Filename { get; set; }

        public string Status { get; set; }

        [Display("Target language")] public string TargetLang { get; set; }
    }
}
