using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Responses
{
    public class DownloadReferenceFilesResponse
    {
        public byte[] File { get; set; }

        public string Filename { get; set; }
    }
}
