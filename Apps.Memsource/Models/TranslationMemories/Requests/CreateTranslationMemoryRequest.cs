using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class CreateTranslationMemoryRequest
    {
        public string Name { get; set; }

        public string SourceLang { get; set; }

        public string TargetLang { get; set; }
    }
}
