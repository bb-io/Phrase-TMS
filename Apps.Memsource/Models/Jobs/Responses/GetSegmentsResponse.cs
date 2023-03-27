using Apps.Memsource.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource.Models.Jobs.Responses
{
    public class GetSegmentsResponse
    {
        public IEnumerable<SegmentDto> Segments { get; set; }
    }
}
