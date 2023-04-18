using Apps.PhraseTms.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Responses
{
    public class JobResponseWrapper
    {
        public IEnumerable<JobDto> Jobs { get; set; }
    }
}
