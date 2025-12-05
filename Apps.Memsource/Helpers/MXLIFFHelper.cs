using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Helpers.Models;
using Blackbird.Filters.Transformations;
using Blackbird.Filters.Xliff.Xliff2;
using System.Xml.Linq;

namespace Apps.PhraseTMS.Helpers;
public class MXLIFFHelper(string baseUrl)
{
    public static readonly XNamespace MNs = "http://www.memsource.com/mxlf/2.0";

    public IEnumerable<MXLIFFUser> GetUsers(Transformation transformation) 
    {
        var extra = transformation.XliffOther.FirstOrDefault(x => x is XElement element && element.Name == MNs + "extra") as XElement;

        if (extra is null) return [];
        var users = extra.Descendants(MNs + "users").FirstOrDefault();
        if (users is null) return [];

        return users.Descendants(MNs + "user").Select(x => new MXLIFFUser(baseUrl)
        {
            Id = x.Get("id"),
            Uid = x.Get("uid"),
            Username = x.Get("username"),
            FullName = x.Get("fullname"),
        });
    }

    public string? GetMetadata(Unit unit, XName key)
    {
        var attribute = unit.Other.FirstOrDefault(x => x is XAttribute attribute && attribute.Name == key) as XAttribute;
        return attribute?.Value;
    }

    public bool IsModified(Unit unit)
    {
        var createdAt = GetMetadata(unit, MXLIFFMetadataKeys.CreatedAt);
        var modifiedAt = GetMetadata(unit, MXLIFFMetadataKeys.ModifiedAt);
        return createdAt != modifiedAt;
    }

    public bool IsConfirmed(Unit unit)
    {
        return GetMetadata(unit, MXLIFFMetadataKeys.Confirmed) == "1";
    }
    public void SetConfirmed(Unit unit, bool confirmed)
    {
        SetMetadata(unit, MXLIFFMetadataKeys.Confirmed, confirmed ? "1" : "0");
    }

    public bool IsLocked(Unit unit)
    {
        return GetMetadata(unit, MXLIFFMetadataKeys.Locked) == "true";
    }
    public void SetLocked(Unit unit, bool locked)
    {
        SetMetadata(unit, MXLIFFMetadataKeys.Locked, locked ? "true" : "false");
    }

    public MXLIFFUser? GetModifiedUser(Unit unit, Transformation transformation)
    {
        var userId = GetMetadata(unit, MXLIFFMetadataKeys.ModifiedBy);
        if (userId is null) return null;
        var allusers = GetUsers(transformation);
        return allusers.FirstOrDefault(x => x.Id == userId);
    }

    public void SetMetadata(Unit unit, XName key, object value)
    {
        var attribute = unit.Other.FirstOrDefault(x => x is XAttribute attribute && attribute.Name == key) as XAttribute;
        if (attribute is null) return;
        attribute.SetValue(value);
    }

    public Unit? FindMatchingMXLIFFUnit(Unit unitInTransformation, Transformation mXliffTransformation)
    {
        foreach (var mXliffGroup in mXliffTransformation.Children.OfType<Group>())
        {
            var contextGroup = mXliffGroup.Other.FirstOrDefault(x => x is XElement element) as XElement;
            var unitId = contextGroup?.Descendants().FirstOrDefault(x => x.Get("context-type") == "x-key")?.Value;

            if (unitId == unitInTransformation.Id)
            {
                return mXliffGroup.GetUnits().FirstOrDefault();
            }
        }

        return null;
    }
}
