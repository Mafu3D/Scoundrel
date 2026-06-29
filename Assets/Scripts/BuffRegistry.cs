using System.Collections.Generic;
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
}