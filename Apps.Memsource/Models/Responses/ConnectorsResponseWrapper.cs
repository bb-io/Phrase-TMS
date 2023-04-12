using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Responses
{
    public class ConnectorsResponseWrapper
    {
        public IEnumerable<ConnectorDto> Connectors { get; set; }
    }
}
