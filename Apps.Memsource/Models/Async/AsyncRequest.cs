using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Async
{
    public class AsyncRequestResponse 
    {
        public AsyncRequest AsyncRequest { get; set; }
    }

    public class AsyncResponse
    {
        public string DateCreated { get; set; }
    }

    public class AsyncRequest
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public string DateCreated { get; set; }
        public AsyncResponse? AsyncResponse { get; set; }

    }
}
