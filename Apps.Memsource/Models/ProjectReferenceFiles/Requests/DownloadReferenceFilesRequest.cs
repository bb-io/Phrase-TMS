using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests
{
    public class DownloadReferenceFilesRequest
    {
        public string ProjectUId { get; set; }

        public string ReferenceFileUId { get; set; }
    }
}
