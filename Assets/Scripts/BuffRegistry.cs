using System.Collections.Generic;
using UnityEngine;

public class GlobalBuffRegistry : MonoBehaviour
{
    [SerializeField] public List<CardBuff> GlobalBuffs;

    // TODO: Refactor this as a hash map instead of a list
    public CardBuff GetBuffFromName(string name)
    {
        foreach (CardBuff buff in GlobalBuffs)
        {
            if (name.ToLower() == buff.Name.ToLower())
            {
                return buff;
            }
        }

        return null;
    }
}