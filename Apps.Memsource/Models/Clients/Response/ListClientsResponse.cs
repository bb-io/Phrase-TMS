using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Clients.Response
{
    public class ListClientsResponse
    {
        public IEnumerable<ClientDto> Clients { get; set; }
    }
}
