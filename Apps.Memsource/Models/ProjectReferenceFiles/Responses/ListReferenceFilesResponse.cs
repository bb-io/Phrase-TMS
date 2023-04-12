using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Responses
{
    public class ListReferenceFilesResponse
    {
        public IEnumerable<ReferenceFileInfoDto> ReferenceFileInfo { get; set; }
    }
}
