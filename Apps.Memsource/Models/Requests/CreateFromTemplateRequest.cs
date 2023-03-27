using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource.Models.Requests
{
    public class CreateFromTemplateRequest
    {
        public string Name { get; set; }

        public string TemplateUId { get; set; }
    }
}
