using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests
{
    public class CreateReferenceFileRequest
    {
        public string ProjectUId { get; set; }

        public byte[] File { get; set; }

        public string Filename { get; set; }
    }
}
