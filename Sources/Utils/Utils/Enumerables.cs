namespace Utils
{
    public static class Enumerables
    {

        public static Dictionary<K, V> GetDictFromLists<K, V>(List<K> keys, List<V> values)
        {
            if (keys == null || values == null || keys.Count == 0 || values.Count == 0)
            {
                return new Dictionary<K, V>();
            }

            return keys.Zip(
                values,
                (key, value) => new { key, value })
                .ToDictionary(kvp => kvp.key, kvp => kvp.value
                );
        }

        public static Dictionary<K, V> FeedListsToDict<K, V>(Dictionary<K, V> kvps, List<K> keys, List<V> values)
        {
            foreach (KeyValuePair<K, V> kvp in GetDictFromLists(keys, values))
            {
                kvps.Add(kvp.Key, kvp.Value);
            }
            return kvps;
        }
    }
}