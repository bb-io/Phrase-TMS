namespace Apps.PhraseTMS.Extension;

public static class DictionaryExtensions
{
    public static Dictionary<T, TV> WithoutNulls<T, TV>(this Dictionary<T, TV> dic) where T : notnull
    {
        return dic
            .Where(x => x.Value != null)
            .ToDictionary(x => x.Key, x => x.Value);
    }
}