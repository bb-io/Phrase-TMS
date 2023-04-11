using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Analysis.Requests
{
    public class CreateAnalysisRequest
    {
        public IEnumerable<string> JobsUIds { get; set; }
    }
}
