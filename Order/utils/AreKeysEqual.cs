namespace Order.utils
{
    public class AreKeysEqual
    {
        public static bool KeysEqual<TKey, TValue>(Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2)
        {
            HashSet<TKey> keys1 = new HashSet<TKey>(dict1.Keys);
            HashSet<TKey> keys2 = new HashSet<TKey>(dict2.Keys);

            return keys1.SetEquals(keys2);
        }
    }
}
