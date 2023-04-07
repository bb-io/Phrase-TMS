using Apps.PhraseTms.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTms.Models.Projects.Requests
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }

        public string SourceLanguage { get; set; }

        public IEnumerable<string> TargetLanguages { get; set; }
    }
}
