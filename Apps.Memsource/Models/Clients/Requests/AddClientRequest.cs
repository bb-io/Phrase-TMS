using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Clients.Requests
{
    public class AddClientRequest
    {
        public string Name { get; set; }

        public string ExternalId { get; set; }
    }
}
