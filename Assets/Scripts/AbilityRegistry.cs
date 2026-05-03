using System.Collections.Generic;
using UnityEngine;

public class AbilityRegistry : MonoBehaviour
{
    [SerializeField] public List<Ability> PublicAbilities;

    public Ability GetAbilityFromName(string name)
    {
        foreach (Ability ability in PublicAbilities)
        {
            if (name.ToLower() == ability.Name.ToLower())
            {
                return ability;
            }
        }

        return null;
    }
}