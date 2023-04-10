using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Files.Responses
{
    public class ListAllFilesResponse
    {
        public IEnumerable<FileInfoDto> Files { get; set; }
    }
}
