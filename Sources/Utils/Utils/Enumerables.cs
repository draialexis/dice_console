namespace Utils
{
    public static class Enumerables
    {

        private static Dictionary<K, V> GetDictFromLists<K, V>(List<K> keys, List<V> values) => keys.Zip(values, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);

        public static Dictionary<K, V> FeedListsToDict<K, V>(Dictionary<K, V> kvps, List<K> keys, List<V> values)
        {
            foreach (var kv in GetDictFromLists(keys, values)) { kvps.Add(kv.Key, kv.Value); }
            return kvps;
        }
    }
}