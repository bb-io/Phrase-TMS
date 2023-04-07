using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTms.Models.Jobs.Responses
{
    public class TargetFileResponse
    {
        public string Filename { get; set; }

        public byte[] File { get; set; }
    }
}
