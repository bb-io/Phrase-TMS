using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.TranslationMemories.Responses
{
    public class ListTranslationMemoriesResponse
    {
        public IEnumerable<TranslationMemoryDto> Memories { get; set; }
    }
}
