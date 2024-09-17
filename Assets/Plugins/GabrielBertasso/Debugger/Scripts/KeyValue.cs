using System;

namespace GabrielBertasso.Debugger
{
    [Serializable]
    public struct KeyValue<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;


        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}