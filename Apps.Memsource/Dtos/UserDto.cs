using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos
{
    public class UserDto
    {
        //public string Id { get; set; }
        public string FirstName { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public string UserName { get; set; }
        public bool Terminologist { get; set; }
        public string UId { get; set; }
        public string Note { get; set; }
        public string LastName { get; set; }
        public string Timezone { get; set; }
        public string Email { get; set; }
    }
}
