using System.Collections.Generic;

public static class DictionaryUtils
{
    public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dictionary, K key, V defaultValue = default)
    {
        if (dictionary.TryGetValue(key, out var value))
            return value;
        return defaultValue;
    }
}