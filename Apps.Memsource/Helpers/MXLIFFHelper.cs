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
        var value = GetMetadata(unit, MXLIFFMetadataKeys.Locked);
        return IsTruthy(value);
    }
    public void SetLocked(Unit unit, bool locked)
    {
        var currentValue = GetMetadata(unit, MXLIFFMetadataKeys.Locked);
        var serializedValue = UsesNumericBoolean(currentValue)
            ? (locked ? "1" : "0")
            : (locked ? "true" : "false");

        SetMetadata(unit, MXLIFFMetadataKeys.Locked, serializedValue);
    }

    public IReadOnlyCollection<string> GetLockedUnitIds(Transformation transformation)
    {
        return transformation.GetUnits()
            .Where(IsLocked)
            .Select(x => x.Id)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToArray();
    }

    public int LockUnits(Transformation transformation, IEnumerable<string> unitIds)
    {
        var unitIdSet = unitIds
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.Ordinal);

        if (unitIdSet.Count == 0)
        {
            return 0;
        }

        var lockedUnitsCount = 0;

        foreach (var unit in transformation.GetUnits().Where(x => unitIdSet.Contains(x.Id)))
        {
            SetLocked(unit, true);
            lockedUnitsCount++;
        }

        return lockedUnitsCount;
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
        if (attribute is null)
        {
            unit.Other.Add(new XAttribute(key, value));
            return;
        }

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

    private static bool IsTruthy(string? value)
    {
        return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) || value == "1";
    }

    private static bool UsesNumericBoolean(string? value)
    {
        return value == "1" || value == "0";
    }
}
