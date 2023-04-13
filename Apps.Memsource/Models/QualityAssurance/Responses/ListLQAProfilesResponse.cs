using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses
{
    public class ListLQAProfilesResponse
    {
        public IEnumerable<LQAProfileDto> Profiles { get; set; }
    }
}
