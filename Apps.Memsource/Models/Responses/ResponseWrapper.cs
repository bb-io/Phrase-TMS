using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Responses
{
    public class ResponseWrapper<T>
    {
        public T Content { get; set; }
    }
}
