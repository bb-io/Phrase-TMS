using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Async
{
    public class AsyncRequestMultipleResponse
    {
        public IEnumerable<AsyncRequestArrayItem> AsyncRequests { get; set; }
    }

    public class AsyncRequestArrayItem
    {
        public AsyncRequest AsyncRequest { get; set; }
    }
}
