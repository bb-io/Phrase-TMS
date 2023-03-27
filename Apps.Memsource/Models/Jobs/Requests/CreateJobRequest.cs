using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource.Models.Jobs.Requests
{
    public class CreateJobRequest
    {
        public string ProjectUId { get; set; }

        public IEnumerable<string> TargetLanguages { get; set; }

        public byte[] File { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }
    }
}
