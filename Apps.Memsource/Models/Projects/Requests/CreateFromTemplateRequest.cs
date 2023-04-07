using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTms.Models.Projects.Requests
{
    public class CreateFromTemplateRequest
    {
        public string Name { get; set; }

        public string TemplateUId { get; set; }
    }
}
