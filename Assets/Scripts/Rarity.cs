using System;
using System.Collections.Generic;
using System.Linq;

public interface IRarity
{
    public Rarity Rarity { get; }
}
public enum Rarity
{
    Common,
    Uncommon,
    Rare
}

public static class RarityExtensions
{
    public static List<T> GetUniqueRandom<T>(this Rarity rarity, List<T> list, int amount, Dictionary<Rarity, int> weights, Random random) where T : IRarity
    {
        List<T> available = new(list);
        List<T> result = new();

        amount = Math.Min(amount, available.Count);

        for (int i = 0; i < amount; i++)
        {
            T selected = rarity.GetWeightedRandom(available, weights, random);
            result.Add(selected);
            available.Remove(selected);
        }

        return result;
    }

    public static T GetWeightedRandom<T>(this Rarity rarity, List<T> list, Dictionary<Rarity, int> weights, Random random) where T :IRarity
    {
        // Sum all of the weights in the collection
        int totalWeight = list.Sum(i => weights[i.Rarity]);

        // Get a random value
        int roll = random.Next(totalWeight);

        // Iterate through the list and subtract the rarity value
        //  from the roll until it gets to the desired item
        foreach(T item in list)
        {
            roll -= weights[item.Rarity];

            if (roll < 0)
            {
                return item;
            }
        }

        return list.Last();
    }
}