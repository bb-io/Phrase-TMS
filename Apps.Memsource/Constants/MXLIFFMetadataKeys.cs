using Apps.PhraseTMS.Helpers;
using System.Xml.Linq;

namespace Apps.PhraseTMS.Constants;
public class MXLIFFMetadataKeys
{
    public static XName Score = MXLIFFHelper.MNs + "score";
    public static XName GrossScore = MXLIFFHelper.MNs + "gross-score";
    public static XName TransOrigin = MXLIFFHelper.MNs + "trans-origin";
    public static XName Confirmed = MXLIFFHelper.MNs + "confirmed";
    public static XName Locked = MXLIFFHelper.MNs + "locked";
    public static XName CreatedAt = MXLIFFHelper.MNs + "created-at";
    public static XName CreatedBy = MXLIFFHelper.MNs + "created-by";
    public static XName ModifiedAt = MXLIFFHelper.MNs + "modified-at";
    public static XName ModifiedBy = MXLIFFHelper.MNs + "modified-by";
    public static XName LevelEdited = MXLIFFHelper.MNs + "level-edited";
}
