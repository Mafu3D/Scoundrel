using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

public class GlobalBuffRegistry : MonoBehaviour
{
    [SerializeField] public List<Buff> GlobalBuffs;
    [SerializeField] public List<PlayerBuff> GlobalPlayerBuffs;

    // TODO: Refactor this as a hash map instead of a list
    public Buff GetBuffFromName(string name)
    {
        foreach (Buff buff in GlobalBuffs)
        {
            if (name.ToLower() == buff.Name.ToLower())
            {
                return buff;
            }
        }

        return null;
    }

    public PlayerBuff GetPlayerBuffFromName(string name)
    {
        foreach (PlayerBuff buff in GlobalPlayerBuffs)
        {
            if (name.ToLower() == buff.Name.ToLower())
            {
                return buff;
            }
        }

        return null;
    }

    public Buff GetRandomBuff(CardType cardType)
    {
        List<Buff> unpackedList = new();
        foreach(Buff buff in GetAllBuffsOfType(cardType))
        {
            int amount = buff.Rarity switch
            {
                Rarity.Common => 3,
                Rarity.Uncommon => 2,
                Rarity.Rare => 1,
                _ => 3,
            };
            for (int i = 0; i < amount; i++)
            {
                unpackedList.Add(buff);
            }
        }

        int randomIndex = Random.Range(0, unpackedList.Count);
        return unpackedList[randomIndex];
    }

    public PlayerBuff GetRandomPlayerBuff()
    {
        // TODO: Make this exclusive? No buff can be gotten twice in a single run. This will require a refactor of the buff system to allow for buff removal from the global registry.
        if (GlobalPlayerBuffs.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, GlobalPlayerBuffs.Count);
        return GlobalPlayerBuffs[randomIndex];
    }

    public List<Buff> GetAllBuffsOfType(CardType cardType)
    {
        return GlobalBuffs.Where(buff => buff.ValidCardTypes.Contains(cardType)).ToList();
    }
}