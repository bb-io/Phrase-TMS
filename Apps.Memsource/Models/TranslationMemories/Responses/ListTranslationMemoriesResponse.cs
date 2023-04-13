using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.TranslationMemories.Responses
{
    public class ListTranslationMemoriesResponse
    {
        public IEnumerable<TranslationMemoryDto> Memories { get; set; }
    }
}
