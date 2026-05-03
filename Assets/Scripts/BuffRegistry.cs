using System.Collections.Generic;
using UnityEngine;

public class BuffRegistry : MonoBehaviour
{
    [SerializeField] public List<Buff> PublicBuffs;

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