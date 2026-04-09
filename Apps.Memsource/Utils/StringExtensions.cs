namespace Apps.PhraseTMS.Utils;

public static class StringExtensions
{
    public static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
        {
            return value;
        }

        return $"{value[..maxLength]}... (truncated)";
    }
}