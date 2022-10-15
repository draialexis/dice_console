namespace Utils
{
    public static class Enumerables
    {

        public static Dictionary<K, V> FromListsToDict<K, V>(List<K> keys, List<V> values)
        {
            return keys.Zip(values, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);
        }
        public static Dictionary<K, V> FeedListsToDict<K, V>(Dictionary<K, V> kvps, List<K> keys, List<V> values)
        {
            Dictionary<K, V> additions = FromListsToDict(keys, values);
            foreach (var kv in additions)
            {
                kvps.Add(kv.Key, kv.Value);
            }

            return kvps;
        }
    }
}