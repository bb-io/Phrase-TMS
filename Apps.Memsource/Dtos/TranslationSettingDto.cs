using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class TranslationSettingDto
{
    [Display("Include tags")] public bool IncludeTags { get; set; }

    [Display("Is pay for MT possible")] public bool PayForMtPossible { get; set; }
    public string Type { get; set; }

    [Display("Settings ID")] public string UId { get; set; }

    //public string Id { get; set; }
    [Display("Is pay for MT active")] public bool PayForMtActive { get; set; }
    [Display("Languages")] public object Langs { get; set; }
    [Display("MT quality estimation")] public bool MtQualityEstimation { get; set; }
    [Display("Base name")] public string BaseName { get; set; }
    [Display("Sharing settings")] public int SharingSettings { get; set; }
    [Display("Char count")] public object CharCount { get; set; }
    public string Name { get; set; }
}