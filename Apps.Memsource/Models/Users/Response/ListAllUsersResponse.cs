using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Users.Response
{
    public class ListAllUsersResponse
    {
        public IEnumerable<UserDto> Users { get; set; }
    }
}
