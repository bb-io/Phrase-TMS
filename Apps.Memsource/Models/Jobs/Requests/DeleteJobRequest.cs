using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTms.Models.Jobs.Requests
{
    public class DeleteJobRequest
    {
        public string ProjectUId { get; set; }

        public IEnumerable<string> JobsUIds { get; set; }
    }
}
