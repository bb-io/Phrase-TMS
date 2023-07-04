using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Clients.Response
{
    public class ListClientsResponse
    {
        public IEnumerable<ClientDto> Clients { get; set; }
    }
}
