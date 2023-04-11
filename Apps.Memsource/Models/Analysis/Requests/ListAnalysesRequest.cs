using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Analysis.Requests
{
    public class ListAnalysesRequest
    {
        public string ProjectUId { get; set; }

        public string JobUId { get; set; }
    }
}
