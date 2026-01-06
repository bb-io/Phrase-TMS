using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models;

public class ConversionResult
{
    public byte[] FileBytes { get; set; }
    public string FileName { get; set; }
    public ImportSettingDto Settings { get; set; }
}