using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos;
public class MinimalUserDto
{
    [Display("User ID")]
    public string UId { get; set; }

    [Display("First name")]
    public string FirstName { get; set; }
    [Display("Last name")]
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
