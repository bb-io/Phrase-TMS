using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Users.Response
{
    public class ListAllUsersResponse
    {
        public IEnumerable<UserDto> Users { get; set; }
    }
}
