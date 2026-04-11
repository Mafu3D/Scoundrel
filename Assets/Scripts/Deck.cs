using System;
using System.Collections.Generic;
using System.Linq;
using Mafu.Extensions;
using UnityEngine;

namespace Project.Decks
{
    public interface IDeckStorable
    {
        public string ID { get; }
    }

    public interface IDeckDefinition<T> where T : IDeckStorable
    {
        public List<T> UnpackContents();
    }

    public class Deck<T> where T : IDeckStorable
    {
        private List<T> allItems = new();
        private List<T> remainingItems = new();
        private Dictionary<int, T> hashedItems = new();
        public int CurrentCount => remainingItems.Count;
        public int TotalCount => allItems.Count;

        public Deck() { }

        public Deck(List<T> items)
        {
            AddPermanent(items, shuffle: false);
        }

        public void AddPermanent(List<T> itemsToAdd, bool shuffle=true, bool addToTop=false)
        {
            foreach (T item in itemsToAdd)
            {
                int hash = HashID(item.ID);
                if (!hashedItems.Keys.Contains(hash))
                {
                    hashedItems.Add(hash, item);
                }

                if (addToTop) allItems.Insert(0, item);
                else allItems.Add(item);
                AddToRemaining(new List<T>() {item}, shuffle, addToTop);
            }
        }

        public void AddToRemaining(List<T> itemsToAdd, bool shuffle=true, bool addToTop=false)
        {
            foreach (T item in itemsToAdd)
            {
                if (item == null) continue;
                if (addToTop) remainingItems.Insert(0, item);
                else remainingItems.Add(item);
                if (shuffle) Shuffle();
            }
        }

        public void Shuffle()
        {
            remainingItems.Shuffle();
        }

        public void Reset()
        {
            remainingItems = new List<T>(allItems);
            Shuffle();
        }

        public bool TryDrawSpecific(string id, out T drawn)
        {
            id = id.ToLower();
            drawn = default;
            if (TryGetItemFromID(id, out T targetItem))
            {
                if (remainingItems.Contains(targetItem))
                {
                    remainingItems.Pop(remainingItems.IndexOf(targetItem), out drawn);
                    return true;
                }
            }
            return false;
        }

        public T Draw(Func<T, bool> filterMethod=null, int index=0)
        {
            if (remainingItems.Count == 0 || index > remainingItems.Count - 1)
            {
                return default;
            }

            T drawnItem;
            remainingItems.Pop(index, out drawnItem);
            List<T> invalidItems = new();
            if (filterMethod != null)
            {
                int maxLoops = 9999;
                int i = 0;
                while(!filterMethod(drawnItem))
                {
                    i++;
                    if (i > maxLoops)
                    {
                        Debug.LogError("Draw reached maximum loops!");
                        break;
                    }
                    invalidItems.Add(drawnItem);
                    if (remainingItems.Count == 0)
                    {
                        Debug.LogError($"{this} is out of cards!");
                    }
                    remainingItems.Pop(index, out drawnItem);
                }
            }

            AddToRemaining(invalidItems);
            Shuffle();

            return drawnItem;
        }

        public List<T> DrawMultiple(int amount=1, Func<List<T>, List<T>> filterMethod=null, bool allowDuplicates=true, int index=0)
        {
            List<T> drawn = DrawAmount(amount, index);
            if (filterMethod != null)
            {
                int maxLoops = 9999;
                int i = 0;
                List<T> invalidItems = filterMethod(drawn);
                while(invalidItems.Count > 0)
                {
                    HashSet<T> sharedItems = new HashSet<T>(drawn.Intersect(invalidItems));
                    drawn.RemoveAll(item => sharedItems.Contains(item));
                    AddToRemaining(new(invalidItems));
                    drawn.AddRange(DrawAmount(amount - drawn.Count, index));
                    i++;
                    if (i > maxLoops)
                    {
                        Debug.LogError("DrawMultiple reached maximum loops!");
                        break;
                    }
                    invalidItems = filterMethod(drawn);
                }
            }
            Shuffle();
            return drawn;
        }

        private List<T> DrawAmount(int amount, int index)
        {
            List<T> drawn = new();
            for (int i = 0; i < amount; i++)
            {
                T item = Draw(index: index); // Don't need to add i since the list is popped
                if (item != null)
                {
                    drawn.Add(item);
                }
            }
            return drawn;
        }

        private bool TryGetItemFromID(string id, out T item)
        {
            int hash = HashID(id);
            return hashedItems.TryGetValue(hash, out item);
        }

        private int HashID(string id) => id.ToLower().ToFNV1aHash();

    }

    public static class Deck {
        public static Deck<T> InitializeDeck<T>(IDeckDefinition<T> deckDefinition) where T : IDeckStorable
        {
            if (deckDefinition == null) return new();
            Deck<T> deck = new Deck<T>(deckDefinition.UnpackContents());
            deck.Reset();
            deck.Shuffle();
            return deck;
        }
    }

    // public class DrawnCollection<T> where T : IDeckStorable
    // {
    //     List<T> drawnItems;
    //     Deck<T> deck;
    //     int amount;

    //     public DrawnCollection(List<T> initialDrawn, Deck<T> ownerDeck)
    //     {
    //         drawnItems = initialDrawn;
    //         amount = drawnItems.Count;
    //         deck = ownerDeck;
    //     }

    //     public DrawnCollection<T> UntilCondition(Func<List<T>, bool> condition)
    //     {
    //         int maxLoops = 9999;
    //         int i = 0;
    //         while(!condition(drawnItems))
    //         {
    //             deck.AddToRemaining(new(drawnItems));
    //             drawnItems = deck.DrawCollection(amount).ToList();
    //             i++;
    //             if (i > maxLoops)
    //             {
    //                 Debug.LogError("DrawUntilCondition reached maximum loops!");
    //                 break;
    //             }
    //         }
    //         return this;
    //     }

    //     public DrawnCollection<T> NoDuplicates()
    //     {
    //         int maxLoops = 9999;
    //         int i = 0;
    //         while(!condition(drawnItems))
    //         {
    //             deck.AddToRemaining(new(drawnItems));
    //             drawnItems = deck.DrawCollection(amount).ToList();
    //             i++;
    //             if (i > maxLoops)
    //             {
    //                 Debug.LogError("DrawUntilCondition reached maximum loops!");
    //                 break;
    //             }
    //         }
    //         return this;
    //     }

    //     public List<T> ToList() => drawnItems;
    // }
}