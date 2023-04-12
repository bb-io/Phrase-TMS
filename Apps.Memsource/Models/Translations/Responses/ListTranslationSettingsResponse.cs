using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Translations.Responses
{
    public class ListTranslationSettingsResponse
    {
        public IEnumerable<TranslationSettingDto> TranslationSettings { get; set; }
    }
}
