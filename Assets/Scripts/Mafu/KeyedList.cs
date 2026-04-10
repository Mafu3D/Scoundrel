using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mafu.Extensions;

namespace Mafu
{
    public class KeyedList<TKey, TValue> : IEnumerable
    {
        private List<KeyValuePair<TKey, TValue>> dataList = new();

        public KeyValuePair<TKey, TValue> this[int i]
        {
            get
            {
                if (i >= 0 && i < dataList.Count)
                {
                    return dataList[i];
                }
                else
                {
                    throw new IndexOutOfRangeException("Index is outside the bounds of the container.");
                }
            }
            set
            {
                dataList[i] = value;
            }
        }

        public int Count => dataList.Count;

        public void Swap(int i1, int i2)
        {
            dataList.Swap(i1, i2);
        }

        public void Add(TKey key, TValue value)
        {
            dataList.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public  KeyValuePair<TKey, TValue> Last()
        {
            return dataList.Last();
        }

        public void Remove(TKey key)
        {
            if (Keys.Contains(key))
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].Key.Equals(key))
                    {
                        dataList.Remove(dataList[i]);
                        return;
                    }
                }
            }
        }

        public void Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (Keys.Contains(keyValuePair.Key))
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].Key.Equals(keyValuePair.Key))
                    {
                        dataList.Remove(dataList[i]);
                        return;
                    }
                }
            }
        }

        public IEnumerator GetEnumerator() => dataList.GetEnumerator();

        public List<TKey> Keys
        {
            get
            {
                List<TKey> keys = new();
                for (int i = 0; i < dataList.Count; i++)
                {
                    keys.Add(dataList[i].Key);
                }
                return keys;
            }
        }

        public List<TValue> Values
        {
            get
            {
                List<TValue> values = new();
                for (int i = 0; i < dataList.Count; i++)
                {
                    values.Add(dataList[i].Value);
                }
                return values;
            }
        }
    }
}