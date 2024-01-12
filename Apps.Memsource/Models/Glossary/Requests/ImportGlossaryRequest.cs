using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Glossary.Requests
{
    public class ImportGlossaryRequest
    {
        [Display("Glossary", Description = "Existing glossary for import")]
        [DataSource(typeof(TermBaseDataHandler))]
        public string GlossaryUId { get; set; }

        [Display("Glossary", Description = "Glossary file exported from other Blackbird apps")]
        public FileReference File { get; set; }
    }
}
