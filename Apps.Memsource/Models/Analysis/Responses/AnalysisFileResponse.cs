using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Analysis.Responses;
public class AnalysisFileResponse
{
    [Display("Analysis file")]
    public FileReference AnalysisFile { get; set; }
}
