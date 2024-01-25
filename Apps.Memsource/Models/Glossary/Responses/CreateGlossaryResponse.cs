using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Glossary.Responses
{
    public class CreateGlossaryResponse
    {
        [Display("Glossary ID")]
        public string GlossaryId { get; set; }
    }
}
