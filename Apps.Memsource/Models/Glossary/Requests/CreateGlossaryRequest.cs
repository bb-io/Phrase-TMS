using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Glossary.Requests
{
    public class CreateGlossaryRequest
    {
        public string Name { get; set; }


        [Display("Glossary languages")]
        [DataSource(typeof(LanguageDataHandler))]
        public IEnumerable<string> Languages { get; set; }
    }
}
