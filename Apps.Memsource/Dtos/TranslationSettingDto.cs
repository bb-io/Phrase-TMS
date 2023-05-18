using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos
{
    public class TranslationSettingDto
    {
        public bool IncludeTags { get; set; }
        public bool PayForMtPossible { get; set; }
        public string Type { get; set; }
        public string UId { get; set; }
        //public string Id { get; set; }
        public bool PayForMtActive { get; set; }
        public object Langs { get; set; }
        public bool MtQualityEstimation { get; set; }
        public string BaseName { get; set; }
        public int SharingSettings { get; set; }
        public object CharCount { get; set; }
        public string Name { get; set; }
    }
}
