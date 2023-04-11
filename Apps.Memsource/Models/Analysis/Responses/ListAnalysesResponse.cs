using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Analysis.Responses
{
    public class ListAnalysesResponse
    {
        public IEnumerable<AnalysisDto> Analyses { get; set; }
    }
}
