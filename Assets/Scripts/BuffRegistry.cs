using System.Collections.Generic;
using UnityEngine;

public class BuffRegistry : MonoBehaviour
{
    [SerializeField] public List<Buff> PublicBuffs;

    // TODO: Refactor this as a hash map instead of a list
    public Buff GetBuffFromName(string name)
    {
        foreach (Buff buff in PublicBuffs)
        {
            if (name.ToLower() == buff.Name.ToLower())
            {
                return buff;
            }
        }

        return null;
    }
}