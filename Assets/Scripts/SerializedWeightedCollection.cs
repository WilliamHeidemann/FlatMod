using System;
using System.Collections.Generic;
using UtilityToolkit.Runtime;

[Serializable]
public class SerializedWeightedCollection<T>
{
    public List<Tuple> Content = new();

    public WeightedCollection<T> Build()
    {
        var collection = new WeightedCollection<T>();
        foreach (var tuple in Content)
        {
            collection.Add(tuple.Item, tuple.Weight);
        }

        return collection;
    }

    [Serializable]
    public class Tuple
    {
        public T Item;
        public float Weight;
    }
}