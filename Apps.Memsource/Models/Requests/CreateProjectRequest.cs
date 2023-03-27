using Apps.Memsource.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource.Models.Requests
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }

        public string SourceLanguage { get; set; }

        public IEnumerable<string> TargetLanguages { get; set; }
    }
}
