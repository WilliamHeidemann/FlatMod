using System;
using System.Collections.Generic;
using UtilityToolkit.Runtime;

[Serializable]
public class SerializedWeightedCollection<T>
{
    public List<T> Items = new();
    public List<float> Weights = new();

    public WeightedCollection<T> Build()
    {
        var collection = new WeightedCollection<T>();
        for (int i = 0; i < Items.Count; i++)
        {
            collection.Add(Items[i], Weights[i]);
        }
        return collection;
    }
}